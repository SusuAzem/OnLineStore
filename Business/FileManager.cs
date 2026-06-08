using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Business
{
    public class FileManager : IFileManager
    {
        private readonly LibraryOptions _options;
       
        public FileManager(IOptions<LibraryOptions> options)
        {
            _options = options.Value;
        }
        public FileStream ImageStream(string image)
        {
            return new FileStream(Path.Combine(_options.Setting!, image), FileMode.Open, FileAccess.Read);
        }

        public async Task<string> SaveImage(IFormFile image)
        {
            try
            {
                var save_path = Path.Combine(_options.Setting!);
                if (!Directory.Exists(_options.Setting))
                {
                    Directory.CreateDirectory(save_path);
                }
                var mine = image.FileName[image.FileName.LastIndexOf('.')..];
                var fileName = $"img_{DateTime.Now:dd-MM-yyyy-HH-mm-ss}{mine}";

                using (var filestream = new FileStream(Path.Combine(save_path, fileName), FileMode.Create))
                {
                    await image.CopyToAsync(filestream);
                }
                return fileName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return "Error";
            }
        }     
    }    
}
