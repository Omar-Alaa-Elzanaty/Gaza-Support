using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gaza_Support.Domains.Models
{
    public class Subscribe
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string DonorId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string SubscribeId { get; set; }
    }
}
