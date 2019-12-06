using CloudPatterns.Managers;
using CloudPatterns.Patterns.CircuitBreaker;
using CloudPatterns.State;
using CloudPatterns.Tests.Mocks;
using NUnit.Framework;
using System.Threading;

namespace CloudPatterns.Tests
{
    [TestFixture]
    public class CircuitBreakerTests
    {
        private CircuitBreaker circuitBreaker;
        private MockSessionState sessionState;
        [OneTimeSetUp]
        public void TestSetup()
        {
            sessionState = new MockSessionState();
            var circuitBreakerStateStore = new CircuitBreakerStateStore(sessionState);
            circuitBreaker = new CircuitBreaker(circuitBreakerStateStore);
        }
        [Test]
        public void ServiceOnCircuitBreakerOn()
        {
            var circuitBreakerManager = new CircuitBreakerManager(circuitBreaker, sessionState);
            circuitBreakerManager.CircuitBreakerEnabled = true;
            circuitBreakerManager.ServiceEnabled = true;
            var result = circuitBreakerManager.PerformAction();
            Assert.IsTrue(result.IsSuccess);
        }
        [Test]
        public void ServiceOnCircuitBreakerOff()
        {
            var circuitBreakerManager = new CircuitBreakerManager(circuitBreaker, sessionState);
            circuitBreakerManager.CircuitBreakerEnabled = false;
            circuitBreakerManager.ServiceEnabled = true;
            var result = circuitBreakerManager.PerformAction();
            Assert.IsTrue(result.IsSuccess);
        }
        [Test]
        public void CircuitBreakerTripAndReset()
        {
            var circuitBreakerManager = new CircuitBreakerManager(circuitBreaker, sessionState);
            circuitBreakerManager.CircuitBreakerEnabled = true;
            circuitBreakerManager.ServiceEnabled = false;
            var result = circuitBreakerManager.PerformAction();
            //see service failure message
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(CircuitBreakerManager.FailureMessage, result.Message);
            //try again to open circuit breaker
            result = circuitBreakerManager.PerformAction();
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(CircuitBreakerManager.FailureMessage, result.Message);
            //try again to see circuit breaker failure message
            result = circuitBreakerManager.PerformAction();
            Assert.IsFalse(result.IsSuccess);
            Assert.AreEqual(CircuitBreakerOpenException.ErrorMessage, result.Message);
            //wait 61 seconds for breaker to reset and set service to on and see success
            Thread.Sleep(61000);
            circuitBreakerManager.ServiceEnabled = true;
            result = circuitBreakerManager.PerformAction();
            Assert.IsTrue(result.IsSuccess);
        }

        [Test]
        public void ServiceOffCircuitBreakerOff()
        {
            var circuitBreakerManager = new CircuitBreakerManager(circuitBreaker, sessionState);
            circuitBreakerManager.CircuitBreakerEnabled = false;
            circuitBreakerManager.ServiceEnabled = false;
            var result = circuitBreakerManager.PerformAction();
            Assert.IsFalse(result.IsSuccess);
        }
    }
}