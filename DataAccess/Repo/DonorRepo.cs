using DataAccess.Interface;
using Gaza_Support.API.Settings;
using Gaza_Support.Domains.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataAccess.Repo
{
    public class DonorRepo : IDonorRepo
    {
        private readonly IMongoCollection<Donor> _donorCollection;
        public DonorRepo(IOptions<MongoDbConfig> options)
        {
            var database = options.Value.GetDataBase();
            _donorCollection = database.GetCollection<Donor>(typeof(User).Name);
        }

        public IMongoCollection<Donor> Collection => _donorCollection;

        public async Task AddAsync(Donor entity)
        {
            await _donorCollection.InsertOneAsync(entity);
        }

        public async Task DeleteAsync(Donor entity)
        {
            await _donorCollection.DeleteOneAsync(x => x.Id == entity.Id);
        }

        public async Task<Donor?> FindOneByAsync(string id)
        {
            var data=await _donorCollection.FindAsync(x => x.Id == id);
            return await data.FirstOrDefaultAsync();
        }

        public async Task ReplaceAsync(Donor entity)
        {
            await _donorCollection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
        }
    }
}