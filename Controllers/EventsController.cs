using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RSVPProAPI.Dtos;
using RSVPProAPI.Models;
using RSVPProAPI.Services;

namespace RSVPProAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IRSVPProService _repo;
        public EventsController(IRSVPProService repo) => _repo = repo;

        [HttpPost("create-event")]
        public async Task<IActionResult> CreateEvent(EventDto dto)
        {
            var newEvent = new Event { Title = dto.Title, EventDate = dto.EventDate, Location = dto.Location, ImageUrl = dto.ImageUrl, Description = dto.Description, Type = dto.Type    };
            await _repo.CreateEventAsync(newEvent);
            return Ok("Event Created.");
        }
    }
}
