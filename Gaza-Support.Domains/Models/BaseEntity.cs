using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gaza_Support.Domains.Models
{
    public class BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
