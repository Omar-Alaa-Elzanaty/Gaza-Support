using Gaza_Support.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Application.Recipients.Queries.GetProfile
{
    public class GetRecipientProfileQueryDto
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
        public ICollection<string> Images { get; set; }
        public List<RecipientContactDto> Contacts { get; set; }
        public List<RecipientPaymentMethodDto> PaymentMethods { get; set; }
    }

    public class RecipientContactDto
    {
        public string ContactType { get; set; }
        public string Contact { get; set; }
    }

    public class RecipientPaymentMethodDto
    {
        public string PaymentName { get; set; }
        public string PaymentInfo { get; set; }
    }
}
