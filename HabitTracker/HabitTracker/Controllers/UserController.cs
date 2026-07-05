using HabitTracker.DataBase.Entities;
using HabitTracker.DataBase.Repositories;
using HabitTracker.Models;
using HabitTracker.Services.Authentication;
using HabitTracker.Services.JWT;
using HabitTracker.Services.PasswordHashing;
using HabitTracker.Services.Profiles;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Security.Claims;

namespace HabitTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IAuthService _authService;

        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet("getname")]
        [Authorize]
        public async Task<IActionResult> GetName()
        {
            ServiceResult<string> servResult = new() { Data = User.FindFirst(ClaimTypes.Name)!.Value };
            return Ok(servResult);
        }

        [HttpPost("reg")]
        public async Task<IActionResult> RegisterAsync([FromBody] RegModel regModel)
        {
            ServiceResult<string> servResult = await _authService.RegisterAsync(regModel);
            if(servResult.Data == null)
            {
                return Conflict(servResult);
            }
            return Ok(servResult);
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromBody] LoginModel loginModel)
        {
            ServiceResult<string> servResult = await _authService.LoginAsync(loginModel);
            if (servResult.Data == null)
            {
                return Conflict(servResult);
            }
            return Ok(servResult);
        }
    }
}
