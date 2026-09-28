using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Dto
{
    public class RegisterUserDto
    {
        public string? ImageSrc { get; set; }
        public String Name { get; set; }
        public String UserName { get; set; }
        public String Email { get; set; }
        public String PhoneNumber { get; set; }
        public String Password { get; set; }
        public String ConfirmPassword { get; set; }
        public string Address { get; set; }
        public String City { get; set; }
        public String State { get; set; }
        public String PostalCode { get; set; }
        public String CnicNumber { get; set; }
        public DateTime CnicExpiry { get; set; }

        //public Boolean IsEmailNotify { get; set; }
        //public Boolean IsPhoneNotify { get; set; }

    }
}
