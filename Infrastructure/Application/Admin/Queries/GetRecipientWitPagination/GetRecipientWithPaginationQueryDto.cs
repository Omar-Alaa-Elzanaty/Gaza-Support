namespace Infrastructure.Application.Admin.Queries.GetRecipientWitPagination
{
    public class GetRecipientWithPaginationQueryDto
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public bool IsVerified { get; set; }
    }
}
