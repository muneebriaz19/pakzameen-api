using Application.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities
{
    public class UserSettings :BaseEntity
    { 
        //public bool IsEmailNotify { get; set; }
        //public bool IsPhoneNotify { get; set; }
        public bool IsAdminVerified { get; set; }
        public bool IsPaid { get; set; }
        public int UserId { get; set; }
        public AppUsers User { get; set; }

    }
}
