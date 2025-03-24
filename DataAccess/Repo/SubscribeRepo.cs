using DataAccess.Interface;
using Gaza_Support.API.Settings;
using Gaza_Support.Domains.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataAccess.Repo
{
    public class SubscribeRepo : ISubscribeRepo
    {
        private readonly IMongoCollection<Subscribe> _subscribeCollection;
        public SubscribeRepo(IOptions<MongoDbConfig> options)
        {
            var database = options.Value.GetDataBase();
            _subscribeCollection = database.GetCollection<Subscribe>(typeof(Subscribe).Name);
        }

        public IMongoCollection<Subscribe> Collection => _subscribeCollection;

        public async Task AddAsync(Subscribe entity)
        {
            await _subscribeCollection.InsertOneAsync(entity);
        }

        public async Task DeleteAsync(Subscribe entity)
        {
            await _subscribeCollection.DeleteOneAsync(x => x.SubscribeId == entity.SubscribeId && x.DonorId == entity.DonorId);
        }

        public async Task<Subscribe?> FindOneByAsync(string subscribeId,string donorId)
        {
            var data = await _subscribeCollection.FindAsync(x => x.SubscribeId == subscribeId && x.DonorId == donorId);
            return await data.FirstOrDefaultAsync();
        }

        public async Task ReplaceAsync(Subscribe entity)
        {
            await _subscribeCollection.ReplaceOneAsync(x => x.SubscribeId == entity.SubscribeId && x.DonorId == entity.DonorId, entity);
        }
    }
}
