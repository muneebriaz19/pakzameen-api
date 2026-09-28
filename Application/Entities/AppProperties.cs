using Application.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities
{
    public class AppProperties:BaseEntity
    {
        public string SaleId { get; set; }
        public int UserId { get; set; }
        public AppUsers User { get; set; }

        public List<PropertyImages> PropertyImages { get; set; }
    }
}
