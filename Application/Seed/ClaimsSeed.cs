using Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Application.Seed
{
    public class ClaimsSeed
    {
        public static void Seed(ModelBuilder builder)
        {
            builder.Entity<IdentityRoleClaim<int>>()
               .HasData(new List<IdentityRoleClaim<int>>
               {
                   new () {
                   Id= 1,
                   RoleId=(int) RoleType.Admin,
                   ClaimType="ChangePassword",
                   ClaimValue="/api/user/changepassword"

                   },

                   new () {
                   Id= 2,
                   RoleId=(int) RoleType.Admin,
                   ClaimType="Refresh",
                   ClaimValue="/api/common/refresh"

                   },
                   new () {
                   Id= 3,
                   RoleId=(int) RoleType.Admin,
                   ClaimType="GetDetails",
                   ClaimValue="/api/user/getdetails"

                   },
                   new () {
                   Id= 4,
                   RoleId=(int) RoleType.User,
                   ClaimType="Refresh",
                   ClaimValue="/api/common/refresh"

                   },
                   new () {
                   Id= 5,
                   RoleId=(int) RoleType.Admin,
                   ClaimType="UpdateUserSettings",
                   ClaimValue="/api/admin/update"

                   },
                   new () {
                   Id= 6,
                   RoleId=(int) RoleType.Admin,
                   ClaimType="DeleteUsers",
                   ClaimValue="/api/user/deleteusers"

                   },
                   new () {
                   Id= 7,
                   RoleId=(int) RoleType.Admin,
                   ClaimType="getUserList",
                   ClaimValue="/api/common/userlist"

                   },
                   new () {
                   Id= 8,
                   RoleId=(int) RoleType.User,
                   ClaimType="GetDetails",
                   ClaimValue="/api/user/getdetails"

                   },
                   new () {
                   Id= 9,
                   RoleId=(int) RoleType.User,
                   ClaimType="ChangePassword",
                   ClaimValue="/api/user/changepassword"

                   },
                    new () {
                   Id= 10,
                   RoleId=(int) RoleType.User,
                   ClaimType="AddProperty",
                   ClaimValue="/api/property/add"

                   },
                     new () {
                   Id= 11,
                   RoleId=(int) RoleType.User,
                   ClaimType="GetProperty",
                   ClaimValue="/api/property/get"

                   },new(){Id= 12,
                   RoleId=(int) RoleType.Admin,
                   ClaimType="AddProperty",
                   ClaimValue="/api/property/add"

                   },
                     new () {
                   Id= 13,
                   RoleId=(int) RoleType.Admin,
                   ClaimType="GetProperty",
                   ClaimValue="/api/property/get"

                   },
                     new () {
                   Id= 14,
                   RoleId=(int) RoleType.Admin,
                   ClaimType="UserList",
                   ClaimValue="/api/user/userlist"

                   },

               });


    }
    }
}
