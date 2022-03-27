using DemoRollGame.DbCore.UoW;
using DemoRollGame.Models.Models;
using DemoRollGame.Models.Requests;
using DemoRollGame.Models.Response;
using DemoRollGame.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace DemoRollGame.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly UserService _userService;
        private IConfiguration _config;
        private readonly ILogger<LoginController> _logger;

        public LoginController(ILogger<LoginController> logger, UserService userService, IConfiguration config)
        {
            _logger = logger;
            _userService = userService;
            _config = config;
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation("user attemps to login");
            var user = _userService.Login(request.UserName, request.Password);
            if (user == null)
            {
                _logger.LogInformation("attempt to login failed");
                return Unauthorized();
            }

            var tokenString = GenerateJSONWebToken(user);
            _logger.LogInformation("attempt to login success");

            return Ok(new LoginResponse() { Token = tokenString, UserId = user.Id.ToString() });
        }

        private string GenerateJSONWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                new Claim(JwtRegisteredClaimNames.Sub, userInfo.UserName),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
