using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Dto
{
    public class ResponseUserListingDto
    {
        public List<UserListingDto> Users { get; set; }
        public int Records { get; set; }
    }
}
