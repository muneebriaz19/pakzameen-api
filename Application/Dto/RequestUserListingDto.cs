using Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class RequestUserListingDto :PaginationDto
    {
        public String? Name { get; set; }
    }
}
