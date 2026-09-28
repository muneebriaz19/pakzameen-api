using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Seed
{
    public class Seed
    {
        public static void Run(ModelBuilder modelBuilder)
        {
            RoleSeed.Seed(modelBuilder);
            ClaimsSeed.Seed(modelBuilder);
        }
    }
}
