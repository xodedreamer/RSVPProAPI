namespace RSVPProAPI.Models
{
    public class Rsvp
    {
        public int Id { get; set; }
        public int EventId { get; set; }
        public int UserId { get; set; }
        public DateTime RsvpDate { get; set; } = DateTime.UtcNow;

        // Logic for "Confirmed" or "Pending" status in your UI
        public string Status { get; set; } = "Pending";

        public Event Event { get; set; } = null!;
        public User User { get; set; } = null!;
    }
}
