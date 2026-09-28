using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entites
{
    public class AppUserRole:IdentityUserRole<int>
    {
        [ForeignKey("UserId")]
        public  AppUsers Users { get; set; }

        [ForeignKey("RoleId")]
        public   AppRole Role { get; set; }
    }
}
