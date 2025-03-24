namespace Infrastructure.Application.Authentication.RecipientLogin
{
    public class RecipientLoginQueryDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string Token { get; set; }
        public bool IsVerified { get; set; }
    }
}
