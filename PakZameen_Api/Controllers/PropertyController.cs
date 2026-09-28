using Application.Dto;
using Application.Interfaces;
using Domain;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace PakZameen_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertyController : BaseController
    {
        // private readonly IUser _user;
         private readonly IProperty _property;

        private readonly IOptions<Settings> options;
        private readonly ICommon _common;
        private readonly Settings settings;
        public PropertyController(IUser user, IOptions<Settings> options, ICommon common,IProperty property)
        {
           // _user = user;
           _property= property;
            this.options = options;
            _common = common;
            settings = options.Value;
        }
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromForm] AddProperty property)
        {
            try
            {
                var response = await _property.Add(property);
                return StatusCode((int)response.StatusCode, response);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        [HttpPost("Get")]
        public async Task<IActionResult> Get([FromForm] GetProperty property)
        {
            try
            {
                var response =  _property.Get(property);
                return StatusCode((int)response.StatusCode, response);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
