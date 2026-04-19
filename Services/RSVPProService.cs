using Microsoft.EntityFrameworkCore;
using RSVPProAPI.Data;
using RSVPProAPI.Dtos;
using RSVPProAPI.Models;

namespace RSVPProAPI.Services
{
    public class RSVPProService : IRSVPProService
    {
       // private readonly IRSVPProService _repo;
        private readonly IFileService _fileService;
        private readonly AppDbContext _context;
        public RSVPProService(IFileService fileService, AppDbContext dbContext)
        {
            _fileService = fileService;
            _context = dbContext;
        }

        // Admin view: Includes User and Event details for tracking
        public async Task<IEnumerable<Rsvp>> GetAllRsvpsAsync()
        {
            return await _context.Rsvps
                .Include(r => r.User)
                .Include(r => r.Event)
                .OrderByDescending(r => r.RsvpDate)
                .ToListAsync();
        }
        public async Task<bool> CreateEventWithImageAsync(CreateEventDto dto)
        {
            // Use the FileService to handle the image
            var imageUrl = await _fileService.SaveImageAsync(dto.image);

            var newEvent = new Event
            {
                Title = dto.Title,
                EventDate = dto.EventDate ?? DateTime.Now,
                ImageUrl = imageUrl,
                Description = dto.Description,
               // Type = dto.Type,
                Location = dto.Location

            };

            // Delegate DB work to the repository
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<User?> GetUserByEmailAsync(string email) =>
        await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public async Task RegisterUserAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> CreateEventAsync(Event newEvent)
        {
            _context.Events.Add(newEvent);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<Event>> GetAllEventsAsync() =>
            await _context.Events.Include(e => e.Rsvps).ToListAsync();

        //User logic

        public async Task<IEnumerable<EventStatusDto>> GetEventsWithUserStatusAsync(int userId)
        {
            return await _context.Events
                .Select(e => new EventStatusDto
                {
                    EventId = e.Id,
                    Title = e.Title,
                    Date = e.EventDate ?? DateTime.MinValue,
                    Location = e.Location,
                    ImageUrl = e.ImageUrl,
                    // Check if any RSVP exists in the Rsvp table for this user and event
                    IsRsvpd = _context.Rsvps.Any(r => r.EventId == e.Id && r.UserId == userId),
                    RsvpStatus = _context.Rsvps
                        .Where(r => r.EventId == e.Id && r.UserId == userId)
                        .Select(r => r.Status)
                        .FirstOrDefault()
                })
                .ToListAsync();
        }

        public async Task<bool> CreateRsvpAsync(Rsvp rsvp)
        {
            // Prevent duplicate RSVPs
            var exists = await _context.Rsvps
                .AnyAsync(r => r.EventId == rsvp.EventId && rsvp.UserId == rsvp.UserId);

            if (exists) return false;

            _context.Rsvps.Add(rsvp);
            return await _context.SaveChangesAsync() > 0;
        }
        public async Task<EventDto> AddEventAsync(EventDto events)
        {
            var newEvent = new Event
            {
                Title = events.Title,
                Description = events.Description,
                Type = events.Type,
                EventDate = events.EventDate,
                Location = events.Location,
                ImageUrl = events.ImageUrl
            };
            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();
            events.Id = newEvent.Id;
            return events;  
        }

        public async Task<EventDetailsDto> addEventDetailsAsync(EventDetailsDto eventDetails)
        {
            var newEvent = new Event
            {
                Title = eventDetails.Title,
                Type = eventDetails.Type,
                EventDate = eventDetails.EventDate
            };
            _context.Events.Add(newEvent);
            await _context.SaveChangesAsync();
            eventDetails.Id = newEvent.Id;
            return eventDetails;
        }

        public async Task<bool> DeleteEventAsync(int eventId)
        {
            var eventToDelete = await _context.Events.FindAsync(eventId);
             if (eventToDelete == null)
                return false;
            _context.Events.Remove(eventToDelete);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<EventDetailsDto> GetEventsByIdAsync(int eventId)
        {
            var events = await _context.Events
                .Where(e => e.Id == eventId)
                .Select(e => new EventDetailsDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Type = e.Type ?? EventType.Wedding, // Replace 'Default' with an appropriate default value
                    EventDate = e.EventDate ?? DateTime.MinValue
                }).FirstOrDefaultAsync();

                return events;
        }

        public async Task<List<EventDetailsDto>> GetEventsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var events = await _context.Events
                .Where(e => e.EventDate >= startDate && e.EventDate <= endDate)
                .Select(e => new EventDetailsDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Type = e.Type ?? EventType.Wedding,
                    EventDate = e.EventDate ?? DateTime.MinValue
                }).ToListAsync(); return events;    
        }

        public async Task<List<EventDetailsDto>> GetEventsByTypeAsync(EventType type)
        {
            var events = await _context.Events
                .Where(e => e.Type == type)
                .Select(e => new EventDetailsDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Type = e.Type ?? EventType.Wedding,
                    EventDate = e.EventDate ?? DateTime.MinValue
                }).ToListAsync(); 
            return events;
        }

        public async Task<bool> UpdateEventAsync(EventDto events)
        {
            var eventToUpdate = await _context.Events.FindAsync(events.Id);
            if (eventToUpdate == null)
                return false;
            eventToUpdate.Title = events.Title;
            eventToUpdate.Description = events.Description;
            eventToUpdate.Type = events.Type;
            eventToUpdate.EventDate = events.EventDate;
            eventToUpdate.Location = events.Location;
            eventToUpdate.ImageUrl = events.ImageUrl;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> updateEventDetailsAsync(int eventId, EventDetailsDto eventDetails)
        {
           var eventToUpdate = _context.Events.Find(eventId);
            if (eventToUpdate == null)
                return false;
            eventToUpdate.Title = eventDetails.Title;
            eventToUpdate.Type = eventDetails.Type;
            eventToUpdate.EventDate = eventDetails.EventDate;
            _context.SaveChanges();
            return true;
        }

       public async Task<EventDto> GetEventByIdAsync(int eventId)
        {
            var events = await _context.Events
                .Where(e => e.Id == eventId)
                .Select(e => new EventDto
                {
                    Id = e.Id,
                    Title = e.Title,
                    Description = e.Description,
                    Type = e.Type ?? EventType.Wedding,
                    EventDate = e.EventDate ?? DateTime.MinValue,
                    Location = e.Location,
                    ImageUrl = e.ImageUrl?? string.Empty
                }).FirstOrDefaultAsync();
                return events;  
        }

        public Task<IEnumerable<Rsvp>> GetRsvpsByEventIdAsync(int eventId)
        {
            throw new NotImplementedException();
        }
    }
}
