
namespace RSVPProAPI.Dtos
{
    public record AuthResponse(
    string Token,
    string FullName,
    string Email,
    string Role // Important for identifying if the user is an Admin
    );
}
