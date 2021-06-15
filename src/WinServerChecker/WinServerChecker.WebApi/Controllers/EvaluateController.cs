using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WinServerChecker.Core;
using WinServerChecker.Core.Exceptions;
using WinServerChecker.Core.Interfaces;
using WinServerChecker.Core.Model;

namespace WinServerChecker.WebApi.Controllers
{
    [Route("/")]
    [ApiController]
    [ResponseCache(NoStore = true)]
    public class EvaluateController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public EvaluateController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get()
        {
            Checker checker = new Checker(_configuration);

            WinServerCheckerResponse wcResponse;
            try
            {
                wcResponse = checker.Process(Request);
            }
            catch (AuthenticationFailedException)
            {
                return StatusCode(StatusCodes.Status403Forbidden);
            }

            if (!wcResponse.OverallPassed)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, wcResponse);
            }

            return Ok(wcResponse);
        }
    }
}
