using AutoMapper;
using FomularOne.Entities;
using FomularOne.Entities.Dtos;
using FomularOne.Repository.Interface;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FomularOne.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AchievementsController : BaseController
    {
        public AchievementsController(IUnitOfWork unitOfWork, IMapper mapper, IMediator mediator) : base(unitOfWork, mapper, mediator)
        {
        }

        [HttpGet]
        [Route("{driverId:guid}")]
        public async Task<IActionResult> GetDriverAchievements(Guid driverId)
        {
            var driverArchievements = await _unitOfWork.Achievements.GetDriverAchievementAsync(driverId);

            if (driverArchievements == null)
                return NotFound("Achievements not found");

            var result = _mapper.Map<DriverAchievementResponse>(driverArchievements);

            return result == null ? NotFound() : Ok(result);
        }

        [HttpPost("")]
        public async Task<IActionResult> AddAchievement([FromBody] CreateDriverAchievementRequest achievement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = _mapper.Map<Achievement>(achievement);

            await _unitOfWork.Achievements.Add(result);

            await _unitOfWork.CompleteAsync();

            return CreatedAtAction(nameof(GetDriverAchievements), new { driverId = result.DriverId }, result);
        }

        [HttpPut("")]
        public async Task<IActionResult> UpdateAchievement([FromBody] UpdateDriverAchievementsRequest achievement)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = _mapper.Map<Achievement>(achievement);

            await _unitOfWork.Achievements.Update(result);

            await _unitOfWork.CompleteAsync();

            return  NoContent();
        }
    }
}
