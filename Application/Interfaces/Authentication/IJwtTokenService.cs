using Application.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Interfaces.Authentication
{
   public interface IJwtTokenService
    {
         TokenDto GenerateJwtToken(int userid,string username,string email , int roleId);
         string ValidateJwtToken(string token);
         string GenerateRefreshToken(string username);
    }
}
