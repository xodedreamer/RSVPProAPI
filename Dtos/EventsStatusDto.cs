namespace RSVPProAPI.Dtos
{
    public class EventStatusDto
    {
        public int EventId { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public bool IsRsvpd { get; set; }
        public string? RsvpStatus { get; set; } // e.g., "Confirmed"
        public string ? Location { get; set; } // Optional: Include location for user convenience
        public string ? ImageUrl { get; set; } // Optional: Include image URL for user convenience
    }
}
