using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class TokenDto
    {
       // public bool isAdmin { get; set; }
       public String JWTToken { get; set; }
       public String RefreshToken { get; set; }
       public DateTime ExpireTime { get; set; }
    }
}
