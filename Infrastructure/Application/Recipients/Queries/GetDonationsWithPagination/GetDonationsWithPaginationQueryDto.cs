using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Application.Recipients.Queries.GetDonationsWithPagination
{
    public class GetDonationsWithPaginationQueryDto
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime Date { get; set; }
        public double Ammount { get; set; }
        public string InvoiceImageUrl { get; set; }
    }
}
