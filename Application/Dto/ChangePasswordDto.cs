using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Models
{
    public class ChangePasswordDto
    {
        public String CurrentPassword { get; set; }
        public String NewPassword { get; set; }
        public String ConfirmPassword { get; set;}
    }
}
