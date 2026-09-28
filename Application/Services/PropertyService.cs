using Application.Dto;
using Application.Entites;
using Application.Interfaces;
using Domain.Common;
using Domain.Interfaces.Authentication;
using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Application.Entities;

namespace Application.Services
{
    public class PropertyService:IProperty
    {
        private readonly Context.SubcrybeContext _userContext;
        //private readonly IJwtTokenService _jwtTokenService;
       // private readonly UserManager<AppUsers> _userManager;
        //private readonly RoleManager<AppRole> _roleManager;
        //private readonly IEmailSender _emailSender;
        //private readonly OTP _otpSettings;
        //private readonly ICommon _common;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PropertyService(Context.SubcrybeContext userContext, IJwtTokenService jwtTokenService, UserManager<AppUsers> userManager, IHttpContextAccessor httpContextAccessor, RoleManager<AppRole> roleManager, IEmailSender emailSender, IOptions<OTP> otpSettings, ICommon common)
        {
            _userContext = userContext;
            //_jwtTokenService = jwtTokenService;
            //_userManager = userManager;
            //_roleManager = roleManager;
            //_emailSender = emailSender;
            //_otpSettings = otpSettings.Value;
            _httpContextAccessor = httpContextAccessor;
            //_common = common;
        }

        public async Task<ResponseDto<string>> Add(AddProperty property)
        {
            var response = new ResponseDto<string>();
            try
            {
                int userId = int.Parse(_httpContextAccessor.HttpContext.Items["userid"].ToString());

                var _property = _userContext.AppProperties.FirstOrDefault(x => x.SaleId == property.SaleId);
                if (_property != null)
                {
                    response.AddError(Messages.PropertyAlreadyExist);
                }
                else
                {
                    var user = _userContext.users.FirstOrDefault(x => x.Id == userId);
                    _property = new AppProperties
                    {
                        SaleId = property.SaleId,
                        UserId = userId,
                        User = user

                        // PropertyImages = new List<PropertyImages> { new PropertyImages { imageUrl = property.ImageUrl[0] } },
                    };

                    var imageList = new List<PropertyImages>();
                    foreach (var item in property.ImageUrl)
                    {
                        imageList.Add(new PropertyImages { imageUrl = item, SaleId = property.SaleId, property = _property });
                    }
                    if(_property.PropertyImages==null)
                    {
                        _property.PropertyImages = new List<PropertyImages>();
                    }
                    _property.PropertyImages.AddRange(imageList);
                    if (user.AppProperties == null)
                    {
                        user.AppProperties = new List<AppProperties>();
                    }
                    user.AppProperties.Add(_property);
                    var result = await _userContext.AppProperties.AddAsync(_property);

                    //await _userContext.PropertyImages.AddRangeAsync(imageList);

                    await _userContext.SaveChangesAsync();
                    response.Message = Messages.PropertyAdded;
                }

            }
            catch (Exception ex)
            {
                response.AddError(ex.Message);
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return response;
        }
        public ResponseDto<AddProperty> Get(GetProperty property)
        {
            var response = new ResponseDto<AddProperty>()
            {
                Result = new AddProperty()
            };
            try
            {
                var images = _userContext.PropertyImages.Where(i => i.SaleId == property.SaleId).Select(x => x.imageUrl).ToList();
                if (images.Count > 0 && images != null)
                {
                    response.Result.SaleId = property.SaleId;
                    response.Result.ImageUrl = images;
                }
                else
                {
                    response.AddError(Messages.RecordNotFound);
                }
            }
            catch (Exception ex)
            {
                response.AddError(ex.Message);
                response.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
            return response;
        }

    }
}
