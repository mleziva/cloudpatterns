using CloudPatterns.Models;
using CloudPatterns.Patterns.CircuitBreaker;
using CloudPatterns.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CloudPatterns.Managers
{
    public class CircuitBreakerManager : ICircuitBreakerManager
    {
        private readonly ISessionState sessionState;

        public static string FailureMessage = "Service failed";
        public string CircuitBreakerState => breaker.State.ToString();
        public bool CircuitBreakerEnabled { get => sessionState.Get<bool>(nameof(CircuitBreakerEnabled)); set => sessionState.Set(nameof(CircuitBreakerEnabled), value); }
        public bool ServiceEnabled { get => sessionState.Get<bool>(nameof(ServiceEnabled)); set => sessionState.Set(nameof(ServiceEnabled), value); }
        private readonly CircuitBreaker breaker;
        public CircuitBreakerManager(CircuitBreaker breaker, ISessionState sessionState)
        {
            this.breaker = breaker;
            this.sessionState = sessionState;
        }
        public ResponseMessageWithStatus PerformAction()
        {
            if (CircuitBreakerEnabled)
            {
                try
                {
                    breaker.ExecuteAction(() =>
                    {
                        return SimulateExternalDependency();
                    });
                }
                catch (CircuitBreakerOpenException ex)
                {
                    return new ResponseMessageWithStatus(false, ex.Message);
                }
                catch (Exception ex)
                {
                    return new ResponseMessageWithStatus(false, ex.Message);
                }
            }
            try
            {
                return SimulateExternalDependency();
            }
            catch (Exception ex)
            {
                return new ResponseMessageWithStatus(false, ex.Message);
            }
        }
      
        private ResponseMessageWithStatus SimulateExternalDependency()
        {
            if (ServiceEnabled)
            {
                Thread.Sleep(200);
                return new ResponseMessageWithStatus(true, "success!");
            }
            Thread.Sleep(2000);
            throw new Exception(FailureMessage);
        }
    }
}
