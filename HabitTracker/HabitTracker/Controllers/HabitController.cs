using HabitTracker.DataBase.Entities;
using HabitTracker.Models;
using HabitTracker.Services.HabitLogics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace HabitTracker.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HabitController : ControllerBase
    {
        private readonly IHabitService _habitService;

        public HabitController(IHabitService habitService)
        {
            _habitService = habitService;
        }

        private int GetUserId()
        {
            return int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
        }

        [HttpGet("getall")]
        public async Task<IActionResult> GetAsync()
        {
            int userId = GetUserId();
            return Ok(await _habitService.GetAllByUserIdAsync(userId));
        }

        [HttpGet("get/{id:int}")]
        public async Task<IActionResult> GetAsync([FromRoute] int id)
        {
            int userId = GetUserId();
            return Ok(await _habitService.GetByIdAsync(id, userId));
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAsync([FromBody] HabitModel habitModel)
        {
            int userId = GetUserId();
            return Ok(await _habitService.CreateAsync(habitModel, userId));
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateAsync([FromBody] HabitModel habitModel)
        {
            int userId = GetUserId();
            ServiceResult<string> servResult = await _habitService.UpdateAsync(habitModel, userId);
            if(servResult.Data == null)
            {
                return NotFound(servResult);
            }
            return Ok(servResult);
        }

        [HttpDelete("delete/{id:int}")]
        public async Task<IActionResult> DeleteAsync([FromRoute] int id)
        {
            int userId = GetUserId();
            ServiceResult<string> servResult = await _habitService.DeleteAsync(id, userId);
            if (servResult.Data == null)
            {
                return NotFound(servResult);
            }
            return NoContent();
        }
    }
}
