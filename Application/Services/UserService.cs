
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
using Domain.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using System.Data;
using static System.Net.WebRequestMethods;

namespace Application.Services
{
    public class UserService : IUser
    {
        private readonly Context.SubcrybeContext _userContext;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly UserManager<AppUsers> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IEmailSender _emailSender;
        private readonly OTP _otpSettings;
        private readonly ICommon _common;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(Context.SubcrybeContext userContext,IJwtTokenService jwtTokenService, UserManager<AppUsers> userManager, IHttpContextAccessor httpContextAccessor, RoleManager<AppRole> roleManager , IEmailSender emailSender, IOptions<OTP> otpSettings,ICommon common)
        {
            _userContext = userContext;
            _jwtTokenService = jwtTokenService;
            _userManager= userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
            _otpSettings = otpSettings.Value;
            _httpContextAccessor = httpContextAccessor;
            _common = common;
        }

        public async Task<ResponseDto<string>> Registration(RegisterUserDto user, RoleType roletype)
        {
            var response = new ResponseDto<String>();
            string otp = String.Empty;
            DateTime otpExpirydateTime;
            bool pending;
            //PendingUsers? pendingUser= null;
            int? ParentId=null;
            try
            {
                //if(roletype==RoleType.User)
                //{
                //    var pendingUser=_userContext.UserApprovals.FirstOrDefault(x=>x.Email==user.Email);
                //    if(pendingUser!=null)
                //    {
                //        ParentId=pendingUser.ParentId;
                //        pending = true;
                //        _userContext.UserApprovals.Remove(pendingUser);
                //    }
                //    else
                //    {
                //        response.AddError("This email has not requested by any of our user");
                //        response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                //        return response;
                //    }
                //}
                var db = await _userManager.FindByEmailAsync(user.Email);
                if (db != null)
                {
                    if (db.EmailConfirmed)
                    {
                        if(db.IsDeleted)
                        {
                            _userContext.users.Remove(db);
                            await _userContext.SaveChangesAsync();
                        }
                        else
                        {
                        response.AddError(Messages.AlreadyRegistered);
                            return response;
                        }
                    }
                    else{
                        otp = Helper.GenerateOTP(_otpSettings.Length);
                        otpExpirydateTime = DateTime.UtcNow.AddMinutes(_otpSettings.Expiry);
                        db.OTP = otp;
                        db.OTPExpiryTime = otpExpirydateTime;
                        sendEmail("Your otp is:" + otp, user.Email);
                        await _userContext.SaveChangesAsync();
                        await _userManager.UpdateAsync(db);
                        response.AddError(Messages.EmailNotVerified);
                        response.AddError(Messages.OTPExpired);
                        return response;
                    }
                    
                }
                var userdetails = _userContext.users.FirstOrDefault(u => u.UserName == user.UserName);
                if (userdetails != null)
                {
                    response.AddError(Messages.UsernameAlreadytaken);
                    return response;
                }
                otp = Helper.GenerateOTP(_otpSettings.Length);
                var role = _roleManager.Roles.FirstOrDefault(x => x.Id == (int)roletype);
                otpExpirydateTime = DateTime.UtcNow.AddMinutes(_otpSettings.Expiry);
                db = new AppUsers
                {
                    Email = user.Email,
                    Name = user.Name,
                    UserName = user.UserName,
                    OTP = otp,
                    PhoneNumber = user.PhoneNumber,
                    OTPExpiryTime = otpExpirydateTime,
                    UserSettings = new UserSettings(),
                    Role=role.Id,
                    //AppUserRole = new List<AppUserRole> { new AppUserRole { RoleId = role.Id } },
                    ImageSrc= user.ImageSrc,
                    Address= user.Address,
                    City= user.City,
                    State= user.State,
                    PostalCode= user.PostalCode,
                    CnicNumber= user.CnicNumber,
                    CnicExpiry= user.CnicExpiry,

                };
                var result = await _userManager.CreateAsync(db, user.Password);
                // Context.Entry(db).Property("CreatedDate").CurrentValue = DateTime.UtcNow;
                //Context.SaveChanges();
                //Context.Admin.Add(db);
                if (!result.Succeeded)
                {
                    response.Errors = result.Errors.Select(x => x.Description).ToList();
                    return response;
                }
                response.Message = Messages.InsertedUser;
                await _userContext.SaveChangesAsync();

                    sendEmail("Your otp is:"+otp,user.Email);
                response.Message = Messages.Inserted;                
            }
            catch (Exception ex)
            {
                response.AddError(ex.Message);
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
                return response;
            }
            return response;

            
        }
        private async void sendEmail(string message,string email)
        {
            await _emailSender.SendEmailAsync(new Email()
            {
                ToEmail = email,
                Subject = "Account Confirmation",
                Body =message,
            });

        }
        public async Task<ResponseDto<TokenDto>> VerifyEmail(VerifyEmailDto model,string Purpose)
        {
            var response = new ResponseDto<TokenDto>();
            try
            {
                //var user =  _userContext.users.FirstOrDefault(i=>i.Email==model.Email);
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    response.AddError(Messages.RecordNotFound);
                    return response;
                }

                if (user.EmailConfirmed && Purpose.Equals("Register"))
                {
                    response.AddError(Messages.EmailConfirmed);
                    return response;
                }

                if (user.OTPExpiryTime < DateTime.UtcNow)
                {
                    response.AddError(Messages.OTPExpired);
                    return response;
                }

                if (model.OTP != user.OTP)
                {
                    response.AddError(Messages.OTPInvalid);
                    return response;
                }
                if (Purpose.Equals("Register"))
                {
                    user.EmailConfirmed = true;
                    await _userManager.UpdateAsync(user);
                    await _userContext.SaveChangesAsync();

                    response.Message = Messages.EmailVerified;
                }
                else if(Purpose.Equals("Login"))
                {
                    var userDetails = _userContext.Users.FirstOrDefault(user => user.Email == model.Email);
                    TokenDto tokenDto = _jwtTokenService.GenerateJwtToken(userDetails.Id, userDetails.UserName, userDetails.Email, userDetails.Role);
                    _common.SaveRefreshToken(userDetails.UserName, tokenDto.RefreshToken);
                    response.Result = tokenDto;
                }
            }
            catch (Exception ex)
            {
                response.AddError(ex.ToString());
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return response;
        }

        public async Task<ResponseDto<string>> Login(LoginDto loginUser)
        {
            var response = new ResponseDto<string>();
            try
            {
                var userDetails = _userContext.Users.FirstOrDefault(user => user.Email == loginUser.Email);
                if (userDetails == null || !await _userManager.CheckPasswordAsync(userDetails, loginUser.Password))
                {
                    response.Errors.Add(Messages.CredentailsInvalid);
                    return response;
                }
                if (!userDetails.EmailConfirmed)
                {
                    response.AddError(Messages.EmailNotVerified);
                    return response;
                }
                //if (!userDetails.usersettings.IsAdminVerified)
                //{
                //    response.AddError(Messages.AdminApproval);
                //    return response;
                //}
                string otp = String.Empty;
                DateTime otpExpirydateTime;
                var db = await _userManager.FindByEmailAsync(loginUser.Email);
                otp = "123456";// Helper.GenerateOTP(_otpSettings.Length);
                otpExpirydateTime = DateTime.UtcNow.AddMinutes(_otpSettings.Expiry);
                db.OTP = otp;
                db.OTPExpiryTime = otpExpirydateTime;
                sendEmail("Your otp is:" + otp, loginUser.Email);
                response.Message = Messages.CheckOTP;
                await _userManager.UpdateAsync(db);
                await _userContext.SaveChangesAsync();
                var db1 = await _userManager.FindByEmailAsync(loginUser.Email);

               // return response;

                //TokenDto tokenDto = _jwtTokenService.GenerateJwtToken(userDetails.Id, userDetails.UserName, userDetails.Email, userDetails.AppUserRole.FirstOrDefault(x => x.UserId == userDetails.Id).RoleId);
                //_common.SaveRefreshToken(userDetails.UserName, tokenDto.RefreshToken);
                //response.Result = tokenDto;
            }
            catch (Exception ex)
            {
                response.Errors.Add(ex.ToString());
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return response;


        }
      

        public async Task<ResponseDto<UserListingDto>> GetDetails()
        {
            var response = new ResponseDto<UserListingDto>();
            try
            {
                int userId = int.Parse(_httpContextAccessor.HttpContext.Items["userid"].ToString());
                var userDetails = _userContext.users.FirstOrDefault(i => i.Id == userId);
                //string roleId = (_httpContextAccessor.HttpContext.Items["roleid"].ToString());
                //var role=_userContext.users.Include(x => x.UserSettings).Include(x => x.AppUserRole).Where(x => (userId == (x.Id))).Select(y=>y.AppUserRole);
                if (userDetails != null)
                {
                    response.Result = new UserListingDto()
                    {
                        Id = userId,
                        role= userDetails.Role,
                        Name = userDetails.Name,
                        UserName = userDetails.UserName,
                        Email = userDetails.Email,
                        EmailConfirmed = userDetails.EmailConfirmed,
                        PhoneNumber = userDetails.PhoneNumber,
                        ImageSrc = userDetails.ImageSrc,
                        // IsAdminVerified = userDetails.UserSettings.IsAdminVerified,
                        Address = userDetails.Address,
                        City = userDetails.City,
                        State = userDetails.State,
                        PostalCode = userDetails.PostalCode,
                        CnicNumber = userDetails.CnicNumber,
                        CnicExpiry = userDetails.CnicExpiry,
                    };
                }
            }
            catch(Exception ex)
            {
                response.Errors.Add(ex.ToString());
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return response;
        }
        public async Task<ResponseDto<ResponseUserListingDto>> UserList()
        {
            var response = new ResponseDto<ResponseUserListingDto>()
            {
                Result = new ResponseUserListingDto() 
            };
            try
            {
                var userlist = _userContext.users.Where(x => x.Role == 2).ToList();
                if(userlist.Count>0&&userlist!=null)
                {
                    response.Result.Users = userlist.Select(user=>new UserListingDto()
                    {
                        Id = user.Id,
                        Email = user.Email,
                        Name = user.Name,
                        UserName = user.UserName,
                        
                        PhoneNumber = user.PhoneNumber,
                        
                        //UserSettings = new UserSettings(),
                        
                        //AppUserRole = new List<AppUserRole> { new AppUserRole { RoleId = role.Id } },
                        ImageSrc = user.ImageSrc,
                        Address = user.Address,
                        City = user.City,
                        State = user.State,
                        PostalCode = user.PostalCode,
                        CnicNumber = user.CnicNumber,
                        CnicExpiry = user.CnicExpiry,

                    }).ToList();
                    response.Result.Records = userlist.Count;
                }
            }
            catch(Exception ex)
            {
                response.Errors.Add(ex.ToString());
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return response;
        }

        //public async Task<ResponseDto<String>> DeleteUser(List<int?> ids, RoleType role)
        //{
        //    var response = new ResponseDto<String>();
        //    try
        //    {
        //        int userId = int.Parse(_httpContextAccessor.HttpContext.Items["userid"].ToString());
        //        var users = _userContext.users.Include(x => x.UserSettings).Include(x => x.AppUserRole).Where(x => (ids.Contains(x.Id) && x.AppUserRole.Any(r => r.RoleId == (int)role) )).ToList();
        //        //users.Append(context.users.Where(z => ids.Contains(z.ParentId)).ToList());        
        //        //Include(x => ids.Contains(x.Id));
        //        if (users == null)
        //        {
        //            response.AddError(Messages.RecordNotFound);
        //            return response;
        //        }

        //        users.ForEach(x => x.IsDeleted = true);
        //        _userContext.Users.UpdateRange(users);
        //        await _userContext.SaveChangesAsync();
        //        response.Message = Messages.RecordDeleted;
        //    }catch(Exception ex)
        //    {
        //        response.Errors.Add(ex.ToString());
        //        response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
        //    }
        //    return response;
        //}

    }
}
