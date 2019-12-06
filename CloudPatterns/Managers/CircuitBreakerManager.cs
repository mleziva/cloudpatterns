using CloudPatterns.Models;
using CloudPatterns.Patterns.CircuitBreaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CloudPatterns.Managers
{
    public class CircuitBreakerManager : ICircuitBreakerManager
    {
        public static string FailureMessage = "Service failed";
        public string CircuitBreakerState => breaker.State.ToString();
        public bool CircuitBreakerEnabled { get; set; } = true;
        public bool ServiceEnabled { get; set; } = true;

        private readonly CircuitBreaker breaker = new CircuitBreaker();
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
