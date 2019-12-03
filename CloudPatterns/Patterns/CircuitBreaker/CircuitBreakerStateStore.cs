using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudPatterns.Patterns.CircuitBreaker
{
    public class CircuitBreakerStateStore : ICircuitBreakerStateStore
    {
        public CircuitBreakerStateEnum State { get; private set; } = CircuitBreakerStateEnum.Closed;

        public Exception LastException { get; private set; }

        public DateTime LastStateChangedDateUtc { get; private set; }

        public bool IsClosed => State == CircuitBreakerStateEnum.Closed;


        public void HalfOpen()
        {
            State = CircuitBreakerStateEnum.HalfOpen;
            LastChangedDateNow();
        }

        public void Reset()
        {
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
