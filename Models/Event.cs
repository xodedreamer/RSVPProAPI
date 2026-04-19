namespace RSVPProAPI.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public EventType? Type { get; set; } = null; // Matches Admin screen picker
        public DateTime? EventDate { get; set; } = null;
        public string Location { get; set; } = string.Empty;
        public string? ImageUrl { get; set; } = string.Empty;
        public int AdminId { get; set; }

        public ICollection<Rsvp> Rsvps { get; set; } = new List<Rsvp>();
    }
}
