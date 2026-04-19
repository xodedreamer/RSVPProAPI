using RSVPProAPI.Models;

namespace RSVPProAPI.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty; // "Admin" or "User"
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties for EF Core
      // public ICollection<Rsvp> Rsvps { get; set; } = new List<Rsvp>();
    }
}
