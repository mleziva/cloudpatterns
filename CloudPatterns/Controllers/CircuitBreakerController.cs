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
        private readonly ICircuitBreakerManager circuitBreakerManager;
        public CircuitBreakerController(ICircuitBreakerManager circuitBreakerManager)
        {
            this.circuitBreakerManager = circuitBreakerManager;
        }
        [HttpGet]
        public IActionResult DoServiceAction()
        {
            var result = circuitBreakerManager.PerformAction();
            return Ok(result);
        }

        [Route("state")]
        [HttpGet]
        public IActionResult GetState()
        {
            var state = new { circuitBreakerManager.ServiceEnabled, circuitBreakerManager.CircuitBreakerEnabled, circuitBreakerManager.CircuitBreakerState };
            return Ok(state);
        }

        [Route("manage/Service")]
        [HttpPut]
        public IActionResult EnableService([FromBody]bool serviceEnabled)
        {
            circuitBreakerManager.ServiceEnabled = serviceEnabled;
            return Ok(serviceEnabled);
        }
        [Route("manage/circuitbreaker")]
        [HttpPut]
        public IActionResult DisableCircuitBreaker([FromBody]bool circuitBreakerEnabled)
        {
            circuitBreakerManager.CircuitBreakerEnabled = circuitBreakerEnabled;
            return Ok(circuitBreakerEnabled);
        }
    }
}