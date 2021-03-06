﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudPatterns.Patterns.CircuitBreaker
{
    public interface ICircuitBreakerStateStore
    {
        CircuitBreakerStateEnum State { get; }

        Exception LastException { get; }

        DateTime LastStateChangedDateUtc { get; }
        DateTime LastExceptionTime { get; set; }
        int FailedRequestCount { get; set; }
        int SuccessRequestCount { get; set; }

        void Trip(Exception ex);

        void Reset();

        void HalfOpen();

        bool IsClosed { get; }

    }
    public enum CircuitBreakerStateEnum
    {
        Closed, HalfOpen, Open
    }
}
