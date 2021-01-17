using ExtCore.Data.Abstractions;
using Microsoft.Extensions.Logging;
using Benriya.Core.Abstractions;
using Benriya.Share.Models.SystemUsers;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Benriya.Utils.Extensions;
using Benriya.Utils;
using Microsoft.Extensions.Options;
using Benriya.Share.ViewModels;
using System;
using Benriya.Core.Abstractions.SystemUsers;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Benriya.Utils.Models;
using MapsterMapper;
using Mapster;
//using TanvirArjel.Extensions.Microsoft.DependencyInjection;

namespace Benriya.Core.Services
{
    //[SingletonService]
    public class UserServices : IUserServices
    {
        private readonly IStorage _storage;
        private readonly ILogger<UserServices> _logger;
        private readonly AppSettings _appSettings;
        private UserInfoModel _user;
        private readonly IUsers_Repository _repo;
        //private readonly IMapper _mapper;
        private readonly JwtOptions _jwtOptions;
        private List<UserPolicyModel> _policies { get; set; }
        public UserServices(IStorage storage, ILogger<UserServices> logger,IOptions<AppSettings> appSettings,IOptions<JwtOptions> jwtOptions)
        {
            _storage = storage;
            _logger = logger;
            _appSettings = appSettings.Value;
            _repo = storage.GetRepository<IUsers_Repository>();
            //_mapper = mapper;
            _jwtOptions = jwtOptions.Value;
        }
        public string Register(Users idata)
        {
            return "okoko";
        }

        public string Login(Users idata)
        {
            return "okoko " + _storage.StorageContext.ToString();
        }



        //----------------
        


        private Users RegisterForm(UserFormModel user)
        {
            return new Users()
            {
                firstname = user.firstname?.Trim(),
                email = user.email.ToLower().Trim(),
                lastname = user.lastname?.Trim(),
                avatar = user.avatar?.Trim(),
                alias_name = user.alias_name?.Trim()
            };
        }
        public async Task<bool> ChangePassword(Guid id, string password)
        {
            var x = await _repo.GetAsync(x=>x.id == id);////.ChangePassword(id, password.Trim());
            return x != null;
        }
        public async Task<UserInfoModel> Register(UserFormModel iuser)
        {
            if (!_appSettings.AllowRegister)
                throw new MethodAccessException("System not available");
            var user = await _repo.CreateAsync(RegisterForm(iuser), iuser.password.Trim());
            if (user == null)
                return null;
            _user = user.Adapt<UserInfoModel>();
            return _user;
        }


        public async Task<UserInfoModel> Authenticate(string username, string password)
        {
            if (username.isNOEOW() || username.Length < 6)
                throw new Exception("Username is required, minimum 6 characters");
            else
                username = Regex.Replace(username, @"\s+", "");

            if (password.isNOEOW() || password.Length < 6)
                throw new Exception("Password is required, minimum 6 characters");
            else
                password = Regex.Replace(password, @"\s+", "");

            var user = await _repo.Authen_Validate(username.Trim(), password.Trim());
            if (user == null)
                throw new Exception("Username or Password invalid");

            UserRoleModel role = new UserRoleModel()
            {
                id = user.User_Role.id,
                name = user.User_Role.name,
                code = user.User_Role.code
            };
            _policies = new List<UserPolicyModel>();
            if(user.User_Role.Policy_Roles != null)
            {
                foreach (var polc in user.User_Role.Policy_Roles)
                {
                    _policies.Add(new UserPolicyModel() { 
                        id = polc.id,code = polc.code,name = polc.name
                    });
                }
            }

            _user = new UserInfoModel() { 
                id = user.id,
                alias_name = user.alias_name,
                avatar = user.avatar,
                email = user.email,
                firstname = user.firstname,
                lastname = user.lastname                
            }; //_mapper.Map<UserInfoModel>(user);
            _user.Role = role;
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var expiry = DateTime.UtcNow.AddHours(_appSettings.loginExpiryHrs);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = this.GenerateClaimsIdentity(),
                Expires = expiry,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtOptions.SecurityKey)), SecurityAlgorithms.HmacSha512Signature)
            };
            _user.Token = new UserTokenModel()
            {
                token = tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor)),
                expiry = expiry
            };
            return _user;
        }

        private ClaimsIdentity GenerateClaimsIdentity()
        {
            if (_user.email.isNOEOW())
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _user.id.ToString()),
                    new Claim(ClaimTypes.Name, _user.id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier,_user.id.ToString()),
                    new Claim(ClaimTypes.Role,_user.Role.code)
                };
                foreach (var polc in _policies)
                {
                    Claim claim = new Claim(ClaimTypes.Role, polc.code);
                    claims.Add(claim);
                }
                return new ClaimsIdentity(claims, "Token");
            }
            else
            {
                var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Sub, _user.email),
                    new Claim(ClaimTypes.Name, _user.id.ToString()),
                    new Claim(ClaimTypes.Email, _user.email),
                    new Claim(ClaimTypes.NameIdentifier,_user.id.ToString()),
                    new Claim(ClaimTypes.Role,_user.Role.code)
                };
                foreach (var polc in _policies)
                {
                    Claim claim = new Claim(ClaimTypes.Role, polc.code);
                    claims.Add(claim);
                }
                return new ClaimsIdentity(claims, "Token");
            }
        }
    }
}
