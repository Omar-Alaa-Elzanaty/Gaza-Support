using Gaza_Support.Domains.Models;
using MongoDB.Bson;

namespace DataAccess.Interface
{
    public interface IUserRepo : IBaseRepo<User>
    {
        Task<User?> FindOneByAsync(string id);
    }
}
