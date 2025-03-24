using Gaza_Support.Domains.Enums;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Gaza_Support.Domains.Models
{
    [BsonDiscriminator(RootClass = true)]
    [BsonKnownTypes(typeof(Donor), typeof(Recipient))]
    public class User : BaseEntity
    {
        public string Email { get; set; }
        public byte[] PasswordHashed { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool IsConfirmedEmail { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }

        #region Recipient
        [BsonRepresentation(BsonType.ObjectId)]
        public string RoleId { get; set; }
        #endregion
    }
}