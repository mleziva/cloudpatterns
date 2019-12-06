using CloudPatterns.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudPatterns.Patterns.CircuitBreaker
{
    public class CircuitBreakerStateStore : ICircuitBreakerStateStore
    {
        private readonly ISessionState sessionState;
        
        public CircuitBreakerStateEnum State { get => sessionState.Get<CircuitBreakerStateEnum>(nameof(State)); private set => sessionState.Set(nameof(State), value); }

        public Exception LastException { get => sessionState.Get<Exception>(nameof(LastException)); private set => sessionState.Set(nameof(LastException), value); }

        public DateTime LastStateChangedDateUtc { get => sessionState.Get<DateTime>(nameof(LastStateChangedDateUtc)); private set => sessionState.Set(nameof(LastStateChangedDateUtc), value); }

        public bool IsClosed => State == CircuitBreakerStateEnum.Closed;

        public int FailedRequestCount { get => sessionState.Get<int>(nameof(FailedRequestCount)); set => sessionState.Set(nameof(FailedRequestCount),value); }
        public int SuccessRequestCount { get => sessionState.Get<int>(nameof(SuccessRequestCount)); set => sessionState.Set(nameof(SuccessRequestCount), value); }
        public DateTime LastExceptionTime { get => sessionState.Get<DateTime>(nameof(LastExceptionTime)); set => sessionState.Set(nameof(LastExceptionTime), value); }
        public CircuitBreakerStateStore(ISessionState sessionState)
        {
            this.sessionState = sessionState;
        }
        public void HalfOpen()
        {
            State = CircuitBreakerStateEnum.HalfOpen;
        }

        public void Reset()
        {
            FailedRequestCount = 0;
            SuccessRequestCount = 0;
            State = CircuitBreakerStateEnum.Closed;
            LastChangedDateNow();
        }

        public void Trip(Exception ex)
        {
            LastException = ex;
            State = CircuitBreakerStateEnum.Open;
            LastChangedDateNow();
        }
        private void LastChangedDateNow()
        {
            LastStateChangedDateUtc = DateTime.UtcNow;
        }
    }
}
