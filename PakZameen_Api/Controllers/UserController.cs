using Application.Dto;
using Application.Interfaces;
using Application.Models;
using Domain.Enums;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace PakZameen_Api.Controllers
{
    public class UserController : BaseController
    {
        private readonly IUser _user;
        private readonly IOptions<Settings> options;
        private readonly ICommon _common;
        private readonly Settings settings;
        public UserController(IUser user, IOptions<Settings> options, ICommon common)
        {
            _user = user;
            this.options = options;
            _common = common;
            settings = options.Value;
        }

        [HttpPost("RegisterAdmin")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterUserDto user)
        {
            try
            {
                var response = await _user.Registration(user, RoleType.Admin);
                return StatusCode((int)response.StatusCode, response);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        [HttpPost("RegisterUser")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody] RegisterUserDto user)
        {
            try
            {
                var response = await _user.Registration(user, RoleType.User);
                return StatusCode((int)response.StatusCode, response);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }


        [HttpPost("verifyRegister")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyRegister([FromBody] VerifyEmailDto model)
        {
            var response = await _user.VerifyEmail(model,"Register");
            return StatusCode((int)response.StatusCode, response);
        }
        [HttpPost("verifyLogin")]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyLogin([FromBody] VerifyEmailDto model)
        {
            var response = await _user.VerifyEmail(model, "Login");
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] LoginDto user)
        {
            try
            {
                Thread.Sleep(1000*60);
                var response = await _user.Login(user);
                return StatusCode((int)response.StatusCode, response);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        [HttpGet("GetDetails")]
        
        public async Task<IActionResult> GetDetails()
        {
            try
            {
                var response = await _user.GetDetails();
                return StatusCode((int)response.StatusCode,response);
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        [HttpGet("UserList")]
        public async Task<IActionResult> UserList()
        {
            try
            {
                var response = await _user.UserList();
                return StatusCode((int)response.StatusCode, response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        [HttpPost("forgetpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ForgetPassword([FromBody] String email)
        {
            try
            {
                var response = await _common.ForgetPassword(email, RoleType.User);
                return StatusCode((int)response.StatusCode, response);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost("resetpassword")]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordDto resetPassword)
        {
            try
            {
                var response = await _common.ResetPassword(resetPassword);
                return StatusCode((int)response.StatusCode, response);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost("changepassword")]
       
        public async Task<IActionResult> ChangePassword(ChangePasswordDto changePassword)
        {
            try
            {
                var response = await _common.ChangePassword(changePassword);
                return StatusCode((int)response.StatusCode, response);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

       
        //[HttpDelete("deleteusers")]
        //public async Task<IActionResult> DeleteUser([FromQuery] List<int?> userList)
        //{
        //    try
        //    {
        //        var response = await _user.DeleteUser(userList, RoleType.User);
        //        return StatusCode((int)response.StatusCode, response);

        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(500, ex);

        //    }
        //}

        
    }
}
