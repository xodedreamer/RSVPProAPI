namespace RSVPProAPI.Services
{
    public interface IFileService
    {
        Task<string?> SaveImageAsync(IFormFile? imageFile);
    }
}
