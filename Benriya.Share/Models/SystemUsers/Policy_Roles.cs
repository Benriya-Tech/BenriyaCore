using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Benriya.Share.Models.SystemUsers
{
    [Table("user_policy_roles")]
    public class Policy_Roles : Common_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [StringLength(64)]
        public string code { get; set; }
        [StringLength(100)]
        public string name { get; set; }
        [StringLength(100)]
        public string module_name { get; set; }
        [StringLength(32)]
        public string module_code { get; set; }
        public string description { get; set; }        
        public bool is_active { get; set; } = true;
        [IndexColumn("IX_Policy_Roles_id",IsUnique = false)]
        public int role_id { get; set; }
        [IndexColumn("IX_Policy_Permission_id",IsUnique = false)]
        public int permission_id { get; set; }
        public virtual User_Role User_Role { get; set; }
        public virtual Permission_Access Permission { get; set; }
    }
}
