using Application.Dto;
using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IProperty
    {
        Task<ResponseDto<string>> Add(AddProperty property);
        ResponseDto<AddProperty> Get(GetProperty property);
    }
}
