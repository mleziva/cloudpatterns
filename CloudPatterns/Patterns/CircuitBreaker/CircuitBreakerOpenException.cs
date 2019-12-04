using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudPatterns.Patterns.CircuitBreaker
{
    public class CircuitBreakerOpenException : Exception
    {
        public static string ErrorMessage = "Circuit Breaker is open";
        public CircuitBreakerOpenException() : base() { }
        public CircuitBreakerOpenException(Exception inner) : base(ErrorMessage, inner) { }
    }
}
