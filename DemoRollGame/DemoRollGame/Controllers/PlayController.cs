using DemoRollGame.DbCore.Repositories.Interfaces;
using DemoRollGame.DbCore.UoW;
using DemoRollGame.Models.Requests;
using DemoRollGame.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace DemoRollGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayController : ControllerBase
    {
        private readonly PlayService _playService;
        private readonly ILogger<LoginController> _logger;
        public PlayController(ILogger<LoginController> logger, PlayService playService)
        {
            _logger = logger;
            _playService = playService;
        }
        [Authorize]
        [HttpPost]
        public IActionResult JoinMatch([FromBody] JoinMatchRequest request)
        {
            var roll = _playService.JoinToGame(Convert.ToInt32(request.UserId));
            if (roll == 0)
                return Unauthorized();
            return Ok(roll);
        }

        [HttpGet("{id}")]
        public IActionResult GetMyRoll([FromRoute] int id)
        {
            var userRoll = _playService.GetUserRoll(id);
            if (userRoll == 0)
                return Unauthorized();
            return Ok(userRoll);
        }
    }
}
