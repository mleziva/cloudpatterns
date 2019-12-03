using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudPatterns.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CloudPatterns.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CircuitBreakerController : ControllerBase
    {
        [HttpGet]
        public IActionResult DoServiceAction()
        {
            var result = CircuitBreakerService.PerformAction();
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetState()
        {
            var state = new { CircuitBreakerService.ServiceEnabled, CircuitBreakerService.CircuitBreakerEnabled };
            return Ok(state);
        }

        [HttpPost]
        public IActionResult EnableService(bool serviceEnabled)
        {
            CircuitBreakerService.ServiceEnabled = serviceEnabled;
            return Ok(true);
        }
        [HttpPost]
        public IActionResult DisableCircuitBreaker(bool circuitBreakerEnabled)
        {
            CircuitBreakerService.CircuitBreakerEnabled = circuitBreakerEnabled;
            return Ok(true);
        }
    }
}