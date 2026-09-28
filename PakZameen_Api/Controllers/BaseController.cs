using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PakZameen_Api.Authorize;

namespace PakZameen_Api.Controllers
{
    //[EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ServiceFilter(typeof(CustomAuthorize))]

    public class BaseController : Controller
    {

    }
}
