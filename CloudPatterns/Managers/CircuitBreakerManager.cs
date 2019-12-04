using CloudPatterns.Models;
using CloudPatterns.Patterns.CircuitBreaker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CloudPatterns.Managers
{
    public static class CircuitBreakerManager
    {
        public static string FailureMessage = "Service failed";
        public static bool CircuitBreakerClosed => breaker.IsClosed;
        public static bool CircuitBreakerEnabled { get; set; } = true;
        public static bool ServiceEnabled { get; set; } = true;

        private static readonly CircuitBreaker breaker = new CircuitBreaker();
        public static ResponseMessageWithStatus PerformAction()
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
      
        private static ResponseMessageWithStatus SimulateExternalDependency()
        {
            if (ServiceEnabled)
            {
                Thread.Sleep(500);
                return new ResponseMessageWithStatus(true, "success!");
            }
            Thread.Sleep(5000);
            throw new Exception(FailureMessage);
        }
    }
}
