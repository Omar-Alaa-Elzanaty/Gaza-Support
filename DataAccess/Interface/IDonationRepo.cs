using Gaza_Support.Domains.Models;
using MongoDB.Bson;

namespace DataAccess.Interface
{
    public interface IDonationRepo : IBaseRepo<Donation>
    {
        Task<Donation?> FindOneByAsync(Donation entity);
    }
}
