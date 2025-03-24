using Gaza_Support.Domains.Dtos;
using Gaza_Support.Domains.Enums;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Infrastructure.IServices
{
    public interface IMediaService
    {
        Task<string?> SaveAsync(MediaFileDto? file, MediaTypes mediaTypes);
        void Delete(string file);
        Task<string?> UpdateAsync(MediaFileDto file, MediaTypes mediaType, string oldUrl);
    }
}
