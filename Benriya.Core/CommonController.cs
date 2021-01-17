using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using Benriya.Utils.Extensions;
using Benriya.Utils.Models;
using Benriya.Share.Abstractions;
using Benriya.Core.Services.LoggingDB;
using Benriya.Share.Models.Clients;
using Microsoft.Extensions.DependencyInjection;

namespace Benriya.Core
{
    public class CommonController : ControllerBase
    {
        private CurrentUser _user;
        protected IRequestServices _request { get; set; }
        public CommonController(IRequestServices request)
        {
            _request = request;
            this._user = this.Current_user();
            _request.SetUser(new CurrentUser() { name = "zzzzzzzzzz",id=_user.id });
            //var logging = request.serviceProvider.GetService<LoggingDBServices>();
            //logging.Create(new RequestLogs() {name=_request.Info.UserAgent});
        }

        
        private Guid Current_user_id()
        {
            string data = HttpContext?.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Name).Value;
            if (!data.isNOEOW())
            {
                try
                {
                    return Guid.Parse(data);
                }
                catch (FormatException)
                {
                    return Guid.Empty;
                }
            }
            return Guid.Empty;
        }

        private CurrentUser Current_user()
        {
            if (_user != null && _user.is_loggedin) return _user;
            Guid id = this.Current_user_id();
            if (id != Guid.Empty)
            {
                this._user = new CurrentUser()
                {
                    is_loggedin = true,
                    id = id
                };
                return this._user;
            }
            return new CurrentUser();
        }
    }
}
