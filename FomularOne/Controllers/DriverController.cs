using AutoMapper;
using FomularOne.Entities;
using FomularOne.Entities.Dtos;
using FomularOne.MessageQueue;
using FomularOne.Repository.Interface;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FomularOne.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DriversController : BaseController
    {
        private readonly IDriverNotificationPublishService _notificationPublishService;
        public DriversController(
            IUnitOfWork unitOfWork, 
            IMapper mapper, 
            IMediator mediator,
            IDriverNotificationPublishService notificationPublishService
            ) : base(unitOfWork, mapper, mediator)
        {
            _notificationPublishService = notificationPublishService;
        }

        [HttpGet]
        [Route("{driverId:guid}")]
        public async Task<IActionResult> GetDriver(Guid driverId)
        {
            var driver = await _unitOfWork.Drivers.GetById(driverId);

            if (driver == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<GetDriverResponse>(driver));
        }

        [HttpPost("")]
        public async Task<IActionResult> AddDriver([FromBody] CreateDriverRequest driver)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _mapper.Map<Driver>(driver);

            await _unitOfWork.Drivers.Add(result);

            await _unitOfWork.CompleteAsync();

            await _notificationPublishService.SendNotification(result.Id, $"{result.FirstName} {result.LastName}");

            return CreatedAtAction(nameof(GetDriver), new { driverId = result.Id }, result);
        }

        [HttpPut("")]
        public async Task<IActionResult> UpdateDriver([FromBody] UpdateDriverRequest updDriver)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = _mapper.Map<Driver>(updDriver);

            await _unitOfWork.Drivers.Update(result);

            await _unitOfWork.CompleteAsync();

            return NoContent();
        }

        [HttpGet]
        public async Task<IActionResult> GetDrivers()
        {
            var drivers = await _unitOfWork.Drivers.All();
            var result = _mapper.Map<IEnumerable<GetDriverResponse>>(drivers);

            return Ok(result);
        }

        [HttpDelete]
        [Route("{driverId:guid}")]
        public async Task<IActionResult> DeleteDriver(Guid driverId)
        {
            var driver = await _unitOfWork.Drivers.GetById(driverId);

            if (driver == null)
                return NotFound();

            await _unitOfWork.Drivers.Delete(driverId);

            await _unitOfWork.CompleteAsync();

            return NoContent();

        }
    }
}
