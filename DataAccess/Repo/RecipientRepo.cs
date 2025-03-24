using DataAccess.Interface;
using Gaza_Support.API.Settings;
using Gaza_Support.Domains.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace DataAccess.Repo
{
    public class RecipientRepo : IRecipientRepo
    {
        private readonly IMongoCollection<Recipient> _recipientCollection;
        public RecipientRepo(IOptions<MongoDbConfig> options)
        {
            var database = options.Value.GetDataBase();
            _recipientCollection = database.GetCollection<Recipient>(typeof(User).Name);
        }

        public IMongoCollection<Recipient> Collection => _recipientCollection;

        public async Task AddAsync(Recipient entity)
        {
            await _recipientCollection.InsertOneAsync(entity);
        }

        public async Task DeleteAsync(Recipient entity)
        {
            await _recipientCollection.DeleteOneAsync(x => x.Id == entity.Id);
        }

        public async Task<Recipient?> FindOneByAsync(string id)
        {
            var data = await _recipientCollection.FindAsync(x => x.Id == id);
            return await data.FirstOrDefaultAsync();
        }

        public async Task ReplaceAsync(Recipient entity)
        {
            await _recipientCollection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
        }
    }
}
