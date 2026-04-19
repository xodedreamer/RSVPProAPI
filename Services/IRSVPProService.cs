using RSVPProAPI.Dtos;
using RSVPProAPI.Models;

namespace RSVPProAPI.Services
{
    public interface IRSVPProService
    {
        //user related methods
        // Auth & User
        Task<User?> GetUserByEmailAsync(string email);
        Task RegisterUserAsync(User user);

        // Event Management (Admin)
        Task<bool> CreateEventAsync(Event newEvent);
        Task<bool> CreateEventWithImageAsync(CreateEventDto dto);
        Task<IEnumerable<Event>> GetAllEventsAsync();
        // Admin: View all RSVPs for tracking
        Task<IEnumerable<Rsvp>> GetAllRsvpsAsync();
        Task<IEnumerable<Rsvp>> GetRsvpsByEventIdAsync(int eventId);

        // User: Create the RSVP record
        Task<bool> CreateRsvpAsync(Rsvp rsvp);
        Task<IEnumerable<EventStatusDto>> GetEventsWithUserStatusAsync(int userId);
        

        // Event Queries
        Task<EventDetailsDto> GetEventsByIdAsync(int eventId);
        Task<List<EventDetailsDto>> GetEventsByTypeAsync(EventType type);
        Task<List<EventDetailsDto>> GetEventsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<EventDetailsDto> addEventDetailsAsync(EventDetailsDto eventDetails);
        Task<bool> updateEventDetailsAsync(int eventId, EventDetailsDto eventDetails);
        Task<bool> DeleteEventAsync(int eventId);

        Task<EventDto> AddEventAsync(EventDto events);
        Task<bool> UpdateEventAsync(EventDto events);
        Task<EventDto> GetEventByIdAsync(int eventId);
    }
}
