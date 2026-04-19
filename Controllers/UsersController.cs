using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RSVPProAPI.Models;
using RSVPProAPI.Services;
using System.Security.Claims;

namespace RSVPProAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IRSVPProService _repo;

        public UsersController(IRSVPProService repo) => _repo = repo;

        [HttpPost("accept/{eventId}")]
        public async Task<IActionResult> AcceptEvent(int eventId)
        {
            // Extract User ID from the token claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (userIdClaim == null) return Unauthorized();

            var rsvp = new Rsvp
            {
                EventId = eventId,
                UserId = int.Parse(userIdClaim),
                RsvpDate = DateTime.Now,
                Status = "Confirmed"
            };

            var success = await _repo.CreateRsvpAsync(rsvp);
            return success ? Ok("RSVP Confirmed") : BadRequest("Failed to RSVP");
        }

        [HttpGet("my-events")]
        public async Task<IActionResult> GetEventsForUser()
        {
            // Extract User ID from the JWT token claims
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userIdClaim))
            {
                return Unauthorized();
            }

            int userId = int.Parse(userIdClaim);
            var events = await _repo.GetEventsWithUserStatusAsync(userId);

            return Ok(events);
        }
    }
}
