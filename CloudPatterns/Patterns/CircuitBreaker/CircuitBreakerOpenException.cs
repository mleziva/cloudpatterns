using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudPatterns.Patterns.CircuitBreaker
{
    public class CircuitBreakerOpenException : Exception
    {
        public CircuitBreakerOpenException() : base() { }
        public CircuitBreakerOpenException(Exception inner) : base("Circuit Breaker is open", inner) { }
    }
}
