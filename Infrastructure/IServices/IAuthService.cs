using Gaza_Support.Domains.Models;
using MongoDB.Bson;

namespace Infrastructure.IServices
{
    public interface IAuthService
    {
        void CreatePasswordHash(User user, string password);
        string GenerateToken(string userId, string email, string roles, bool rembemerMe = false, bool isMobile = false);
        bool VerifyPassword(User user, string password);
    }
}
