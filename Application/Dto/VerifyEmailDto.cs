using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public  class VerifyEmailDto
    {
        public string Email { get; set; }
        public string OTP { get; set; }
        //public string Purpose { get; set; }
    }
}
