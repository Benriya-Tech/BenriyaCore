using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Benriya.Share.Models.SystemUsers
{

    [Table("permission_access")]
    public class Permission_Access : Common_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }

        [StringLength(64)]
        [IndexColumn("IX_Permission_Access_Code",IsUnique = true)]
        public string code { get; set; }
        [StringLength(100)]
        public string name { get; set; }
        public string description { get; set; }        

        public bool is_active { get; set; } = true;

        public virtual ICollection<Policy_Roles> Policy_Roles { get; set; }
    }
}
