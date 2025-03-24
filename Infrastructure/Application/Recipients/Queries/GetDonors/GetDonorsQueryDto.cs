namespace Infrastructure.Application.Recipients.Queries.GetDonors
{
    public class GetDonorsQueryDto
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Country { get; set; }
    }
}
