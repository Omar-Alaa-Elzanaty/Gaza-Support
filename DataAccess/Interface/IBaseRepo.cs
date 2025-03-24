using Gaza_Support.Domains.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataAccess.Interface
{
    public interface IBaseRepo<T>
    {
        IMongoCollection<T> Collection { get; }
        Task AddAsync(T entity);
        Task DeleteAsync(T entity);
        Task ReplaceAsync(T entity);
    }
}
