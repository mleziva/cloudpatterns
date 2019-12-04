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
        public static bool ServiceEnabled { get; set; }
        public static bool CircuitBreakerEnabled { get; set; }
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
            return SimulateExternalDependency();

        }

        private static ResponseMessageWithStatus SimulateExternalDependency()
        {
            if (ServiceEnabled)
            {
                Thread.Sleep(500);
                return new ResponseMessageWithStatus(true, "success!");
            }
            Thread.Sleep(5000);
            throw new Exception("Service failed");
        }
    }
}
