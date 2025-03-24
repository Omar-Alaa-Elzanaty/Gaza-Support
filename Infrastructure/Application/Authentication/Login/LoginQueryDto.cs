namespace Infrastructure.Application.Authentication.Login
{
    public class LoginQueryDto
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Country { get; set; }
        public string Token { get; set; }
    }
}
