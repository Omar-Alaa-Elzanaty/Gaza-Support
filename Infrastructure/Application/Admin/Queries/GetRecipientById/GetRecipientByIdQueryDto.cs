using Gaza_Support.Domains.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Application.Admin.Queries.GetRecipientById
{
    public class GetRecipientByIdQueryDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string MiddleName { get; set; }
        public string Country { get; set; }
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
        public List<string> Images { get; set; }
    }
}
