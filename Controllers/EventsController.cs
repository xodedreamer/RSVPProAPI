using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RSVPProAPI.Dtos;
using RSVPProAPI.Models;
using RSVPProAPI.Services;
using System.Security.Claims;

namespace RSVPProAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IRSVPProService _repo;
        public EventsController(IRSVPProService repo) => _repo = repo;

        [HttpPost("create-event")]
        public async Task<IActionResult> CreateEvent(EventDto dto)
        {
            // Extract User ID from the JWT token claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized();
            }

            var newEvent = new Event { Title = dto.Title, 
                EventDate = dto.EventDate, 
                Location = dto.Location, 
                ImageUrl = dto.ImageUrl, 
                Description = dto.Description,
                Type = dto.Type,
                AdminId = int.Parse(userIdClaim)
            };
            await _repo.CreateEventAsync(newEvent);
            return Ok("Event Created.");
        }
    }
}
