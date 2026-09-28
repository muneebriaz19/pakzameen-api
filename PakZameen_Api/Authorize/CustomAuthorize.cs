using Application.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Dto;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace PakZameen_Api.Authorize
{
   
    public class CustomAuthorize : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly SubcrybeContext _dBContext;

        public CustomAuthorize(SubcrybeContext dBContext)
        {
            _dBContext = dBContext;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (HasAllowAnonymous(context))
                return;
            var response = new ResponseDto<string>();
            var route = context.HttpContext.Request.Path;
            var identity = context.HttpContext.User.Identity as ClaimsIdentity;
            if (identity != null)
            {
               // var Username = context.HttpContext.Items["username"];
                
                IEnumerable<Claim> claims = identity.Claims;
                if (!claims.Any() )
                {
                    // not logged in
                    response.Result = "Unauthorized";
                    response.Success = false;
                    response.StatusCode = System.Net.HttpStatusCode.Unauthorized;
                    context.Result = new UnauthorizedObjectResult(response);
                    // context.Result = new JsonResult(new { message = "Unauthorized" }) { StatusCode = StatusCodes.Status401Unauthorized };
                }
                else
                {
                    var username=claims.First(x => x.Type == "username")?.Value;
                    var userid = claims.First(x => x.Type == "userid")?.Value;
                    var roleid = claims.FirstOrDefault(a => a.Type == "roleid")?.Value;
                    var roleClaim = _dBContext.RoleClaims.Where(r => r.RoleId == int.Parse(roleid)).Select(x => x.ClaimValue.ToLower()).ToList();
                    if (!roleClaim.Contains(route.Value.ToLower()) || username == null)
                    {
                        response.Result = "Forbidden";
                        response.Success = false;
                        response.StatusCode = System.Net.HttpStatusCode.Forbidden;
                        context.Result = new UnauthorizedObjectResult(response);
                    }
                    else
                    {
                        context.HttpContext.Items["username"] = username;
                        context.HttpContext.Items["userid"] = userid;
                    }
                }
            }

        }

        private static bool HasAllowAnonymous(AuthorizationFilterContext context)
        {
            var filters = context.ActionDescriptor.EndpointMetadata.ToList();

            foreach (var filter in filters)
            {
                if (filter.ToString().Contains("AllowAnonymousAttribute"))
                {
                    return true;
                }
            }
            return false;
        }

    }
}
