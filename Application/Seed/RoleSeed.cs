using Application.Entites;
using Application.Models;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Application.Seed
{
    public class RoleSeed
    {
        public static void Seed(ModelBuilder builder)
        {
            builder.Entity<AppRole>()
              .HasData(
                  new List<AppRole>
                  {
                        new()
                        {
                            Id = (int) RoleType.Admin,
                            Name = "Admin",
                            NormalizedName = "ADMIN",
                            ConcurrencyStamp = "955ccc3f-87b2-4c4a-a4e6-2aae679d6196"
                        },
                       
                         new()
                        {
                            Id = (int) RoleType.User,
                            Name = "User",
                            NormalizedName = "USER",
                            ConcurrencyStamp = "77481246-d605-4fe7-977d-5904403d2cdb"
                        },

                  });

        }
    }
}
