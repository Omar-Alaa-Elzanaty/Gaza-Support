﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gaza_Support.Domains.Models
{
    public class Donation
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string RecipientId { get; set; }
        [BsonRepresentation(BsonType.ObjectId)]
        public string DonorId { get; set; }
        public string InvoiceImageUrl { get; set; }
        public double Amount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string? Note { get; set; }
        public bool IsRead { get; set; }
    }
}
