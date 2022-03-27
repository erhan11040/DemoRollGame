using DemoRollGame.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System;

namespace DemoRollGame.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MatchController : ControllerBase
    {
        private readonly MatchService _matchService;
        private readonly ILogger<LoginController> _logger;
        public MatchController(ILogger<LoginController> logger, MatchService matchService)
        {
            _logger = logger;
            _matchService = matchService;
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult GetMatches()
        {
            return Ok(_matchService.GetAllMatches());
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public IActionResult GetMatchById(int id)
        {
            return Ok(_matchService.GetMatchById(id));
        }

        [HttpPost]
        public IActionResult AddMatch()
        {
            throw new NotImplementedException();
        }

        [HttpPatch]
        public IActionResult EditMatch()
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        public IActionResult DeleteMatch()
        {
            throw new NotImplementedException();
        }
    }
}
