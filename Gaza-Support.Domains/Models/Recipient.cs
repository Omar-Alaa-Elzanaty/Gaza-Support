using Gaza_Support.Domains.Enums;
using MongoDB.Bson;

namespace Gaza_Support.Domains.Models
{
    public class Recipient : User
    {
        public string NationalId { get; set; }
        public string? NationalIdUrl { get; set; }
        public string? Story { get; set; }
        public string? CasePublicVideoUrl { get; set; }
        public string? CasePrivateVideoUrl { get; set; }
        public DateOnly? LastCheckPrivateVideo { get; set; }
        public string? EgyptionMobileWallet { get; set; }
        public string? BankAccountIBAN { get; set; }
        public string? InstaPayUserName { get; set; }
        public string? LinkedInLink { get; set; }
        public string? FacebookLink { get; set; }
        public string? TwitterLink { get; set; }
        public string? InstagramLink { get; set; }
        public string? GoFundMeLink { get; set; }
        public bool IsVerified { get; set; }
        public RecipientStatus Status { get; set; }
        public ICollection<RecipientContact> Contacts { get; set; }
        public ICollection<string> Images { get; set; }
        public ICollection<RecipientPaymentMethod> PaymentMethods { get; set; }
    }
}
