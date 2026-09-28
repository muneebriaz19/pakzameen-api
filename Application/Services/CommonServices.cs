using Application.Context;
using Application.Dto;
using Application.Entites;
using Application.Entities;
using Application.Interfaces;

using Application.Models;

using Domain;
using Domain.Common;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Application.Services
{
    public class CommonServices:ICommon
    {
        private readonly Context.SubcrybeContext _dbContext;
        private readonly UserManager<AppUsers> _userManager;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailSender _emailSender;
        private readonly ResetPassword _resetpassword;

        public CommonServices(Context.SubcrybeContext dbContext, UserManager<AppUsers> userManager, IJwtTokenService jwtTokenService, IHttpContextAccessor httpContextAccessor , IEmailSender emailSender, IOptions<ResetPassword> resetpassword) {
            _dbContext = dbContext;
            _userManager = userManager;
            _jwtTokenService = jwtTokenService;
            _httpContextAccessor = httpContextAccessor;
            _emailSender = emailSender;
            _resetpassword = resetpassword.Value;
        }
       
        public string GetRefreshToken(string username)
        {
            String token = String.Empty;
            try
            {
                var user=_dbContext.RefreshTokens.FirstOrDefault(t => t.Username == username);
                if(user!= null)
                {
                    token=user.RefToken;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
            return token;
        }

        public void SaveRefreshToken(string username,string refreshToken)
        {
            try
            {
                var user = _dbContext.RefreshTokens.FirstOrDefault(t => t.Username == username);
                RefreshToken token = null;
                if (user == null)
                {
                    token = new RefreshToken()
                    {
                        Username = username,
                        TokenId = new Random().Next().ToString(),
                        RefToken = refreshToken,
                        IsActive = true

                    };
                    _dbContext.RefreshTokens.Add(token);
                    _dbContext.Entry(token).State = EntityState.Added;
                }
                else
                {
                    user.RefToken = refreshToken;
                    _dbContext.Entry(token).State = EntityState.Modified;
                }

                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }
            
        }

        //public ResponseDto<TokenDto> RefreshToken(string refreshToken)
        //{
        //    var response = new ResponseDto<TokenDto>();
        //    try
        //    {
                
        //        string username = _httpContextAccessor.HttpContext.Items["username"].ToString();
        //        var user = _dbContext.Users.Include(x=>x.AppUserRole).FirstOrDefault(s => s.UserName == username);
        //        if (user == null)
        //        {
        //            response.AddError(Messages.NeedLogin);
        //        }
        //        var token = _dbContext.RefreshTokens.FirstOrDefault(token => token.Username.Equals(username) && token.RefToken==refreshToken);
        //        if(token == null)
        //        {
        //            response.AddError(Messages.InvalidToken);
        //        }
        //        var tokenDto = _jwtTokenService.GenerateJwtToken(user.Id,username, user.Email, user.AppUserRole.FirstOrDefault().RoleId);
        //        SaveRefreshToken(username, tokenDto.RefreshToken);
        //        response.Result= tokenDto;
              
                
        //    }
        //    catch (Exception ex)
        //    {

        //        Console.WriteLine(ex.Message);
        //        response.AddError(ex.Message);
        //    }
        //    return response;

        //}

        public async Task<ResponseDto<String>> ForgetPassword(string email,RoleType role)
        {
            var response=new ResponseDto<String>();
            var user=_dbContext.Users.FirstOrDefault(s => s.Email == email && s.Role == (int)role);
            if(user == null)
            {
                response.AddError(Messages.EmailNotFound);
                return response;
            }
            if (!user.EmailConfirmed)
            {
                response.AddError(Messages.EmailNotVerified);
                return response;
            }
            var resetToken= HttpUtility.UrlEncode(await _userManager.GeneratePasswordResetTokenAsync(user));
            var userresetToken = _dbContext.ResetPasswordTokens.FirstOrDefault(usertoken => usertoken.UserId == user.Id);
            if (userresetToken == null)
            {
                var resetpasswordtoken = new ResetPasswordTokens()
                {
                    UserId = user.Id,
                    isUsed = false,
                    ResetToken = resetToken,
                    Expiry = DateTime.UtcNow.AddMinutes(_resetpassword.Expiry)
                };
                _dbContext.ResetPasswordTokens.Add(resetpasswordtoken);
            }
            else
            {
                userresetToken.ResetToken = resetToken;
                userresetToken.isUsed = false;
                userresetToken.Expiry = DateTime.UtcNow.AddMinutes(_resetpassword.Expiry);
                _dbContext.Update(userresetToken);
            }
          
            await _dbContext.SaveChangesAsync();
            var resetLink = _resetpassword.Link.ToString().Replace("{{token}}", resetToken);
            sendEmail(resetLink);
            response.Message = Messages.ResetPasswordLink;
            return response;

              
            async void sendEmail(string link)
            {
                await _emailSender.SendEmailAsync(new Email()
                {
                    ToEmail = user.Email,
                    Subject = "ResetPassword",
                    Body = "Your reset password link is : "+link,
                });

            }
        }
    
        public async Task<ResponseDto<String>> ResetPassword(ResetPasswordDto resetPasswordDto) {
        var response = new ResponseDto<String>();
        var user= await _userManager.FindByEmailAsync(resetPasswordDto.Email);
            if (user == null)
            {
                response.AddError(Messages.EmailNotFound);
                return response;

            }
            if (!user.EmailConfirmed)
            {
                response.AddError(Messages.EmailConfirmed);
                return response;
            }
            var resettoken=_dbContext.ResetPasswordTokens.FirstOrDefault(x=>x.UserId==user.Id);
            if (!resettoken.ResetToken.Equals(resetPasswordDto.Token)|| resettoken.isUsed)
            {
                response.AddError(Messages.InvalidToken);
                return response;
            }
            if (resettoken.Expiry < DateTime.UtcNow)
            {
                response.AddError(Messages.TokenExpired);
            }
          
            var result =await _userManager.ResetPasswordAsync(user, HttpUtility.UrlDecode(resettoken.ResetToken),resetPasswordDto.Password);

            if(!result.Succeeded) {
                response.Errors = result.Errors.Select(x => x.Description).ToList();
                return response;

            }
            resettoken.isUsed = true;
            _dbContext.Update(resettoken);
            await _dbContext.SaveChangesAsync();
            response.Message=Messages.PasswordReset;
            return response;
        
        
        }
        public async Task<ResponseDto<String>> ChangePassword(ChangePasswordDto changePasswordDto)
        {
            var response=new ResponseDto<String>();
            string username = _httpContextAccessor.HttpContext.Items["username"].ToString();
            if (username == null)
            {
                response.AddError(Messages.NeedLogin);
                return response;
            }
           // var user=_dbContext.Users.AsNoTracking().FirstOrDefault(x=>x.UserName== username);
            var user =_userManager.Users.FirstOrDefault(x=>x.UserName==username);
            if (user == null || !await _userManager.CheckPasswordAsync(user,changePasswordDto.CurrentPassword))
            {
                response.Errors.Add(Messages.OldPasswordNotMatched);
                return response;
            }
            var result =await _userManager.ChangePasswordAsync(user, changePasswordDto.CurrentPassword, changePasswordDto.NewPassword);
            if (!result.Succeeded)
            {
                response.Errors = result.Errors.Select(x => x.Description).ToList();
                return response;
            }
            //await _userManager.UpdateAsync(result);
            await _dbContext.SaveChangesAsync();
            response.Message=Messages.PasswordReset;
            return response;


        }
    }
}
