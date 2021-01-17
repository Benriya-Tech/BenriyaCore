using Benriya.Share.Models.SystemUsers;
using Benriya.Utils.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace Benriya.Share.ViewModels
{
    public class UsersEditModel : Users
    {

        [Compare(nameof(email), ErrorMessage = "Confirm email don't match.")]
        public string confirm_email { get; set; }

        //[Required]
        public string password { get; set; }
        [Compare(nameof(password), ErrorMessage = "Passwords don't match.")]
        public string confirm_password { get; set; }
    }

    public class UserFormModel
    {        
        [StringLength(100)]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string email { get; set; }
        public string firstname { get; set; }
        public string password { get; set; }
        public string lastname { get; set; }
        public string avatar { get; set; }
        public string alias_name { get; set; }
    }

    public class UserLoginModel
    {
        [StringLength(100)]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string email { get; set; }
        public string password { get; set; }     
        public bool remember_me { get; set; }
    }




    public class UserInfoModel
    {
        public Guid id { get; set; }
        public string email { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string avatar { get; set; }
        public string alias_name { get; set; }
        public UserRoleModel Role { get; set; }
        public UserTokenModel Token { get; set; }
    }

    public class UserTokenModel
    {
        public Guid id { get; set; }
        public string token { get; set; }
        public DateTime expiry { get; set; }
    }
    public class UserRoleModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public ICollection<UserPolicyModel> Policies { get; set; }
    }

    public class UserPolicyModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
    }
}
