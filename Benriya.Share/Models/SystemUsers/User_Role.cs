using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Benriya.Share.Models.SystemUsers
{
    //[Serializable]
    [Table("user_roles")]
    public class User_Role : Common_Model_Force
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [IndexColumn("IX_Rolecode",IsUnique = true)]
        [StringLength(32)]
        [Required]
        public string code { get; set; }
        [StringLength(100)]
        [Required]
        public string name { get; set; }
        public string description { get; set; }
        public bool is_active { get; set; } = true;
        [IndexColumn("IX_RoleLevel",IsUnique = false)]
        [Range(0, 30)]
        public int role_level { get; set; } = 0;

        public virtual ICollection<Users> User { get; set; }
        public virtual ICollection<Policy_Roles> Policy_Roles { get; set; }
    }
}
