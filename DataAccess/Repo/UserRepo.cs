using DataAccess.Interface;
using Gaza_Support.API.Settings;
using Gaza_Support.Domains.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataAccess.Repo
{
    public class UserRepo : IUserRepo
    {
        private readonly IMongoCollection<User> _userCollection;
        public UserRepo(IOptions<MongoDbConfig> options)
        {
            var database = options.Value.GetDataBase();
            _userCollection = database.GetCollection<User>(typeof(User).Name);
        }

        public IMongoCollection<User> Collection => _userCollection;
        public async Task AddAsync(User entity)
        {
            await _userCollection.InsertOneAsync(entity);
        }

        public async Task DeleteAsync(User entity)
        {
            await _userCollection.DeleteOneAsync(x => x.Id == entity.Id);
        }

        public async Task<User?> FindOneByAsync(string id)
        {
            var data = await _userCollection.FindAsync(x => x.Id == id);
            return await data.FirstOrDefaultAsync();
        }

        public async Task ReplaceAsync(User entity)
        {
            await _userCollection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
        }
    }
}