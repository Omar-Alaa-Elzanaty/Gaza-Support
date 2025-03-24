using MongoDB.Driver;

namespace Gaza_Support.API.Settings
{
    public class MongoDbConfig
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }

        public IMongoDatabase GetDataBase()
        {
            var client = new MongoClient(ConnectionString);
            return client.GetDatabase(DatabaseName);
        }
    }
}