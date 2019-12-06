using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudPatterns.Models;
using CloudPatterns.Patterns.CircuitBreaker;

namespace CloudPatterns.Managers
{
    public interface ICircuitBreakerManager
    {
        string CircuitBreakerState { get; }
        bool CircuitBreakerEnabled { get; set; }
        bool ServiceEnabled { get; set; }
        public ResponseMessageWithStatus PerformAction();
    }
}
