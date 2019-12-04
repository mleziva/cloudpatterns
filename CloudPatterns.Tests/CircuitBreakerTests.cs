using CloudPatterns.Managers;
using CloudPatterns.Patterns.CircuitBreaker;
using NUnit.Framework;
using System.Threading;

namespace CloudPatterns.Tests
{
    public class CircuitBreakerTests
    {
        

        [Test]
        public void ServiceOnCircuitBreakerOn()
        {
            CircuitBreakerManager.CircuitBreakerEnabled = true;
            CircuitBreakerManager.ServiceEnabled = true;
            var result = CircuitBreakerManager.PerformAction();
            Assert.IsTrue(result.IsSuccess);
        }
        [Test]
        public void ServiceOnCircuitBreakerOff()
        {
            CircuitBreakerManager.CircuitBreakerEnabled = false;
            CircuitBreakerManager.ServiceEnabled = true;
            var result = CircuitBreakerManager.PerformAction();
            Assert.IsTrue(result.IsSuccess);
        }
        [Test]
        public void CircuitBreakerTripAndReset()
        {
            CircuitBreakerManager.CircuitBreakerEnabled = true;
            CircuitBreakerManager.ServiceEnabled = false;
            var result = CircuitBreakerManager.PerformAction();
            //see service failure message
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(CircuitBreakerManager.FailureMessage, result.Message);
            //try again and see circuit breaker message
            result = CircuitBreakerManager.PerformAction();
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(CircuitBreakerOpenException.ErrorMessage, result.Message);
            //wait 61 seconds for breaker to reset and set service to on and see success
            Thread.Sleep(61000);
            CircuitBreakerManager.ServiceEnabled = true;
            result = CircuitBreakerManager.PerformAction();
            Assert.IsTrue(result.IsSuccess);
        }

        [Test]
        public void ServiceOffCircuitBreakerOff()
        {
            CircuitBreakerManager.CircuitBreakerEnabled = false;
            CircuitBreakerManager.ServiceEnabled = false;
            var result = CircuitBreakerManager.PerformAction();
            Assert.IsFalse(result.IsSuccess);
        }
    }
}