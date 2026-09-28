using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Entities
{
    public class PropertyImages:BaseEntity
    {
        public string SaleId { get; set; }
        public AppProperties property { get; set; }
        public string imageUrl { get; set; }
    }
}
