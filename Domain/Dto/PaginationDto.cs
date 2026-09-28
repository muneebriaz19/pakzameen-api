using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Dto
{
    public class PaginationDto
    {
        public PaginationDto()
        {
            PageNumber = 1;
            PageSize = 50;
            SortOrder = (int)SortDirection.DESC;
        }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int? SortOrder { get; set; }
    }
}
