using DataAccess.Interface;
using Gaza_Support.API.Settings;
using Gaza_Support.Domains.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataAccess.Repo
{
    public class RoleRepo : IRoleRepo
    {
        private readonly IMongoCollection<Role> _roleCollection;
        public RoleRepo(IOptions<MongoDbConfig> options)
        {
            var database = options.Value.GetDataBase();
            _roleCollection = database.GetCollection<Role>(typeof(Role).Name);
        }
        public IMongoCollection<Role> Collection => _roleCollection;

        public async Task AddAsync(Role entity)
        {
            await _roleCollection.InsertOneAsync(entity);
        }

        public async Task DeleteAsync(Role entity)
        {
            await _roleCollection.DeleteOneAsync(x => x.Id == entity.Id);
        }

        public async Task<Role?> FindOneByAsync(string id)
        {
            var data = await _roleCollection.FindAsync(x => x.Id == id);
            return await data.FirstOrDefaultAsync();
        }

        public async Task ReplaceAsync(Role entity)
        {
            await _roleCollection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
        }
    }
}
