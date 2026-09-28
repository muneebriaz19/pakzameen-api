using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Common
{
    public  class ResetPassword
    {
        public string Link { get; set; }
        public int Expiry { get; set; }
    }
}
