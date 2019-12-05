using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CloudPatterns.Patterns.CircuitBreaker
{
    public class CircuitBreaker
    {
        private readonly ICircuitBreakerStateStore stateStore = new CircuitBreakerStateStore();

        private readonly object halfOpenSyncObject = new object();

        //wait 1 minute before retrying if breaker is open
        private readonly TimeSpan openToHalfOpenWaitTime = new TimeSpan(0, 0, 1, 0);
        public bool IsClosed { get { return stateStore.IsClosed; } }

        public bool IsOpen { get { return !IsClosed; } }
        public CircuitBreakerStateEnum State => stateStore.State;
        public T ExecuteAction<T>(Func<T> action)
        {
            if (IsOpen)
            {
                // The circuit breaker is Open. Check if the Open timeout has expired.
                // If it has, set the state to HalfOpen. Another approach might be to
                // check for the HalfOpen state that had be set by some other operation.
                if (stateStore.LastStateChangedDateUtc + openToHalfOpenWaitTime < DateTime.UtcNow)
                {
                    return LockAndPerformAction(action);
                }
                // The Open timeout hasn't yet expired. Throw a CircuitBreakerOpen exception to
                // inform the caller that the call was not actually attempted,
                // and return the most recent exception received.
                throw new CircuitBreakerOpenException(stateStore.LastException);
            }


            // The circuit breaker is Closed, execute the action.
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                // If an exception still occurs here, simply
                // retrip the breaker immediately.
                this.TrackException(ex);

                // Throw the exception so that the caller can tell
                // the type of exception that was thrown.
                throw;
            }
        }

        private void TrackException(Exception ex)
        {
            // For simplicity in this example, open the circuit breaker on the first exception.
            // In reality this would be more complex. A certain type of exception, such as one
            // that indicates a service is offline, might trip the circuit breaker immediately.
            // Alternatively it might count exceptions locally or across multiple instances and
            // use this value over time, or the exception/success ratio based on the exception
            // types, to open the circuit breaker.
            stateStore.FailedRequestCount++;
            var previousLastExceptionTime = stateStore.LastExceptionTime;
            stateStore.LastExceptionTime = DateTime.UtcNow;
            if (stateStore.FailedRequestCount > 1 && previousLastExceptionTime.AddMinutes(1) > stateStore.LastExceptionTime)
            {
                stateStore.Trip(ex);
            }
            
        }
        private T LockAndPerformAction<T>(Func<T> action)
        {
            // The Open timeout has expired. Allow one operation to execute. Note that, in
            // this example, the circuit breaker is set to HalfOpen after being
            // in the Open state for some period of time. An alternative would be to set
            // this using some other approach such as a timer, test method, manually, and
            // so on, and check the state here to determine how to handle execution
            // of the action.
            // Limit the number of threads to be executed when the breaker is HalfOpen.
            // An alternative would be to use a more complex approach to determine which
            // threads or how many are allowed to execute, or to execute a simple test
            // method instead.
            bool lockTaken = false;
            try
            {
                Monitor.TryEnter(halfOpenSyncObject, ref lockTaken);
                if (lockTaken)
                {
                    stateStore.HalfOpen();
                    var result = action();
                    stateStore.SuccessRequestCount++;
                    if(stateStore.SuccessRequestCount > 1)
                    {
                        this.stateStore.Reset();
                    }
                    return result;
                }
                return default;
            }
            catch (Exception ex)
            {
                // If there's still an exception, trip the breaker again immediately.
                this.stateStore.Trip(ex);
                // Throw the exception so that the caller knows which exception occurred.
                throw;
            }
            finally
            {
                if (lockTaken)
                {
                    Monitor.Exit(halfOpenSyncObject);
                }
            }
        }
    }
}
