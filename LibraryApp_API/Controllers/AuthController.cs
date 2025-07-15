using LibraryApp_API.Services;
using LibrarySystemWPF.Models;
using Microsoft.AspNetCore.Mvc;
using System;

namespace LibraryApp_API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _userService;
        private readonly JwtService _jwtService;

        public AuthController(UserService userService, JwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            var user = _userService.GetUserByUsername(model.Username);
            if (user == null || user.Password != model.Password)
                return Unauthorized("Invalid credentials");

            if (user.StatusDesc != "Active")
                return Unauthorized("Account is inactive");

            var token = _jwtService.GenerateToken(user);
            return Ok(new { Token = token, User = user });
        }
    }

    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}