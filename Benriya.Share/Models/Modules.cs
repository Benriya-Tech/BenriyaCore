//using Benriya.Share.Models.SystemUsers;
//using System;
//using System.Collections.Generic;
//using System.ComponentModel.DataAnnotations;
//using System.ComponentModel.DataAnnotations.Schema;
//using System.Text;
//using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

//namespace Benriya.Share.Models
//{
//    [Table("module_info", Schema = "modules")]
//    public class Modules : Common_Model
//    {
//        [Key]
//        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
//        public int id { get; set; }

//        [StringLength(64)]
//        [IndexColumn("IX_Modules_Code",IsUnique = true)]
//        public string code { get; set; }
//        [StringLength(100)]
//        public string name { get; set; }
//        public bool is_active { get; set; } = true;
//        public string url { get; set; }
//        public string helper { get; set; }
//        public string version { get; set; }
//        public string autors { get; set; }
//        public virtual ICollection<Policy_Roles> Policy_Roles { get; set; }
//    }
//}
