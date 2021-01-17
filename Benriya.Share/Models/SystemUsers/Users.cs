using Benriya.Utils.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Benriya.Share.Models.SystemUsers
{
    [Table("users")]
    public class Users : User_Common
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public string avatar { get; set; }
        public bool is_active { get; set; } = true;
        public int role_id { get; set; }
        [NotMapped]
        public string full_name
        {
            get
            {
                return this.firstname.isNOEOW() ? this.alias_name : this.firstname + " " + this.lastname;
            }
        }
        public virtual User_Role User_Role { get; set; }
        public virtual User_Credential User_Credential { get; set; }
        public virtual ICollection<User_Directauth> Direct_Auth { get; set; }
        public virtual User_Token User_Token { get; set; }
        public virtual User_Login User_Login { get; set; }
    }


    public class User_Common : Common_Model_Force
    {
        public string alias_name { get; set; }
        [StringLength(100)]
        [Required(ErrorMessage = "The email address is required")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string email { get; set; }
        [StringLength(100)]
        public string firstname { get; set; }
        [StringLength(100)]
        public string lastname { get; set; }
    }

}
