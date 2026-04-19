using RSVPProAPI.Models;

namespace RSVPProAPI.Dtos
{
    public class CreateEventDto
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public EventType? Type { get; set; }
        public DateTime? EventDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
        public IFormFile? image { get; set; } // Optional: For image upload handling
    }
}
