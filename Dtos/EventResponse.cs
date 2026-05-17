namespace RSVPProAPI.Dtos
{
    public record EventResponse(
           int Id,
           string Title,
           string Description,
           string Type, // Returns string representation ("Wedding", "Conference", etc.)
           DateTime EventDate,
           string Location,
           string ImageUrl
       );
}
