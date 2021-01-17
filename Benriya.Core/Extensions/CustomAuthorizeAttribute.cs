using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;
using Benriya.Utils.Extensions;
using Benriya.Utils;

namespace Benriya.Core.Extensions
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class ClientAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        private readonly string _permissionCliam;

        public ClientAuthorizeAttribute(string module, string filterParameter)
        {
            _permissionCliam = module + "." + filterParameter;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;            
            //var xxx = _permissionCliam;
            if (!user.Identity.IsAuthenticated)
            {
                var result = new ApiResultModel<bool>();
                result.Unauthorized();           
                context.Result = new JsonResult(result);
                context.HttpContext.Response.StatusCode = result.Status;
                return;
            }
            //return;
            //// you can also use registered services
            //var someService = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var alowAccess = user.Claims.Where(x => x.Value.Equals(_permissionCliam)).FirstOrDefault();
            if (alowAccess == null || alowAccess.Value.isNOEOW())
            {
                var result = new ApiResultModel<bool>();
                result.Forbidden();          
                context.Result = new JsonResult(result);
                context.HttpContext.Response.StatusCode = result.Status;
                return;
            }
            return;
        }
    }
}

