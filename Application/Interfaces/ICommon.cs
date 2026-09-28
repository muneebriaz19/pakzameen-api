
using Application.Dto;
using Application.Entites;
using Application.Models;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface ICommon
    {
        void SaveRefreshToken(string username, string refreshToken);
        string GetRefreshToken(string username);
        //ResponseDto<TokenDto> RefreshToken(string refreshToken);
        Task<ResponseDto<String>> ForgetPassword(string email, RoleType role);
        Task<ResponseDto<String>> ResetPassword(ResetPasswordDto resetPasswordDto);
        Task<ResponseDto<String>> ChangePassword(ChangePasswordDto changePasswordDto);

        
        
    }
}
