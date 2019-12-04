using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudPatterns.Managers;
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
            var result = CircuitBreakerManager.PerformAction();
            return Ok(result);
        }

        [HttpGet]
        public IActionResult GetState()
        {
            var state = new { CircuitBreakerManager.ServiceEnabled, CircuitBreakerManager.CircuitBreakerEnabled };
            return Ok(state);
        }

        [HttpPost]
        public IActionResult EnableService(bool serviceEnabled)
        {
            CircuitBreakerManager.ServiceEnabled = serviceEnabled;
            return Ok(true);
        }
        [HttpPost]
        public IActionResult DisableCircuitBreaker(bool circuitBreakerEnabled)
        {
            CircuitBreakerManager.CircuitBreakerEnabled = circuitBreakerEnabled;
            return Ok(true);
        }
    }
}