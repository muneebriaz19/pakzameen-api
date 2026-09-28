using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class UserListingDto
    {
        public int Id { get; set; }
        public string ImageSrc { get; set; }
        public int role { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }

        public string PhoneNumber { get; set; }
        public bool EmailConfirmed { get; set; }

        public bool Ispaid { get; set; }
        public bool IsAdminVerified { get; set; }
        public string Address { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        public String PostalCode { get; set; }
        public String CnicNumber { get; set; }
        public DateTime CnicExpiry { get; set; }
    }
}
