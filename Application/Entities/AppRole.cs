using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entites
{
    public class AppRole:IdentityRole<int>
    {
       public List<AppUserRole>? UserRole { get; set; }
    }
}
