using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities
{
    public  class RefreshToken
    {
        public string Username { get; set; }
       
        public string TokenId { get; set; }
        public string RefToken { get; set; }
        public bool IsActive { get; set; }
    }
}
