using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using LinearProgrammingProblems.Models;
using LinearProgrammingProblems.Services;
using Newtonsoft.Json;

namespace LinearProgrammingProblems.Controllers
{
    public class ProblemsController : ControllerBase
    {
        private readonly ILogger<ProblemsController> _logger;

        public ProblemsController(ILogger<ProblemsController> logger)
        {
            _logger = logger;
        }

        [HttpPost("api/solve")]
        public IActionResult Post([FromBody]TransportProblem transportProblem)
        {
            if (ModelState.IsValid)
            {
                TransportProblemService transportProblemService = new TransportProblemService(transportProblem);
                var res = transportProblemService.Solve();
                var json = JsonConvert.SerializeObject(res);
                return Ok(json);
            }
            return UnprocessableEntity();
        }
    }
}