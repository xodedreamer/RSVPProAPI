using RSVPProAPI.Models;

namespace RSVPProAPI.Dtos
{
    public class EventDetailsDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public EventType Type { get; set; }
        public DateTime EventDate { get; set; }

        public List<RsvpDto> Attendees { get; set; } = new();
    }
}
