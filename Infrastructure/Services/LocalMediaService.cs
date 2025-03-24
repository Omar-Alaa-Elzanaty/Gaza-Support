using Gaza_Support.Domains.Dtos;
using Gaza_Support.Domains.Enums;
using Infrastructure.IServices;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class LocalMediaService : IMediaService
    {
        private readonly IWebHostEnvironment _host;
        private readonly IConfiguration _configuration;

        public LocalMediaService(
            IWebHostEnvironment hostingEnvironment,
            IConfiguration configuration)
        {
            _host = hostingEnvironment;
            _configuration = configuration;
        }

        public void Delete(string file)
        {
            var path = Path.Combine(_host.WebRootPath, file);

            if (File.Exists(path))
            {
                File.Delete(path);
            }
        }

        public async Task<string?> SaveAsync(MediaFileDto? media, MediaTypes mediaType)
        {
            if (media == null || media.Base64 == null || media.Base64.Length == 0)
                return null;

            var extension = Path.GetExtension(media.FileName);
            var file = Guid.NewGuid().ToString() + extension;

            var fileRootPath = GetFilePath(mediaType);

            var path = Path.Combine("wwwroot", fileRootPath, file);

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            await File.WriteAllBytesAsync(path, Convert.FromBase64String(media.Base64));

            return Path.Combine(fileRootPath, file);
        }

        public async Task<string?> UpdateAsync(MediaFileDto media, MediaTypes mediaType, string oldUrl)
        {
            if (oldUrl == null && media == null)
            {
                return null;
            }

            if (media == null)
            {
                return oldUrl;
            }

            if (oldUrl == null)
            {
                return await SaveAsync(media, mediaType);
            }

            Delete(oldUrl);
            return await SaveAsync(media, mediaType);
        }

        private string GetFilePath(MediaTypes mediaType)
        {
            return mediaType switch
            {
                MediaTypes.Image => _configuration["MediaSavePath:ImagePath"]!,
                MediaTypes.Vidoe => _configuration["MediaSavePath:VideoPath"]!,
                _ => "",
            };
        }
    }
}
