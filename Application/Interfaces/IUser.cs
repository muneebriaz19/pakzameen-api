using Application.Dto;
using Application.Entities;
using Application.Models;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
   public interface IUser
    {
        Task<ResponseDto<String>> Registration(RegisterUserDto user, RoleType roletype);
        Task<ResponseDto<string>> Login(LoginDto loginUser);
        Task<ResponseDto<TokenDto>> VerifyEmail(VerifyEmailDto model,string Purpose);

        Task<ResponseDto<UserListingDto>> GetDetails();
        Task<ResponseDto<ResponseUserListingDto>> UserList();
        //ResponseDto<String> AddJob(AddJobDto requestDto);

        //Task<ResponseDto<String>> AddUser(AddUser addUser);

        //ResponseDto<ResponseUserJobsDto> ViewJobs();
        //Task<ResponseDto<String>> DeleteUser(List<int?> ids, RoleType role);
        //Task<ResponseDto<String>> DeleteJob(List<int?> ids);
        //Task<ResponseDto<String>> PauseNotify(int id);
        //Task<ResponseDto<String>> ResumeNotify(int id);


        // JObject Login(Login user);

        //  Task<JObject> DownloadFile(String path);
    }
}
