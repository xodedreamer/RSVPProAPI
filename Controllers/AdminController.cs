using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RSVPProAPI.Dtos;
using RSVPProAPI.Models;
using RSVPProAPI.Services;
using System.Security.Claims;

namespace RSVPProAPI.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IRSVPProService _repo;

        public AdminController(IRSVPProService repo) => _repo = repo;

        [HttpPost("create-event")]
        public async Task<IActionResult> CreateEvent([FromForm] CreateEventDto dto)
        {

            var success = await _repo.CreateEventWithImageAsync(dto);

            if (!success) return BadRequest("Could not create event.");

            return Ok(new { message = "Event created successfully!" });
        }

        [HttpGet("view-rsvps")]
        public async Task<IActionResult> GetReports()
        {
            var events = await _repo.GetAllEventsAsync();
            return Ok(events);
        }

        [HttpGet("track-signups")]
        public async Task<IActionResult> GetAllSignUps()
        {
            var rsvps = await _repo.GetAllRsvpsAsync();

            // Map to a clean DTO for the admin dashboard
            var report = rsvps.Select(r => new {
                EventName = r.Event.Title,
                AttendeeEmail = r.User.Email,
                DateSignedUp = r.RsvpDate,
                r.Status
            });

            return Ok(report);
        }
    }
}
