using DataAccess.Interface;
using Gaza_Support.API.Settings;
using Gaza_Support.Domains.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace DataAccess.Repo
{
    public class DonationRepo : IDonationRepo
    {
        private readonly IMongoCollection<Donation> _donationCollection;
        public DonationRepo(IOptions<MongoDbConfig> options)
        {
            var database = options.Value.GetDataBase();
            _donationCollection = database.GetCollection<Donation>(typeof(Donation).Name);
        }

        public IMongoCollection<Donation> Collection => _donationCollection;

        public async Task AddAsync(Donation entity)
        {
            await _donationCollection.InsertOneAsync(entity);
        }

        public async Task DeleteAsync(Donation entity)
        {
            await _donationCollection.DeleteOneAsync(x => x.DonorId==entity.DonorId&&x.RecipientId==entity.RecipientId);
        }

        public async Task<Donation?> FindOneByAsync(Donation entity)
        {
            var data = await _donationCollection.FindAsync(x => x.DonorId == entity.DonorId && x.RecipientId == entity.RecipientId);
            return await data.FirstOrDefaultAsync();
        }

        public async Task ReplaceAsync(Donation entity)
        {
            await _donationCollection.ReplaceOneAsync(x => x.DonorId == entity.DonorId && x.RecipientId == entity.RecipientId, entity);
        }
    }
}
