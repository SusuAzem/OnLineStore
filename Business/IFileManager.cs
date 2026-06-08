using Microsoft.AspNetCore.Http;

namespace Business
{
    public interface IFileManager
    {
        FileStream ImageStream(string image);
        Task<string> SaveImage(IFormFile image);
    }
}
