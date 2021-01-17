using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Benriya.Share.Models.Menus
{
    [Table("menu_system")]
    public class SystemMenu : Common_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [StringLength(100)]
        [IndexColumn("IX_Menu_Name",IsUnique = false)]
        [Required]
        public string name { get; set; }
        [IndexColumn("IX_Menu_Code", IsUnique = true)]
        [Required]
        public string code { get; set; }
        public string url { get; set; }
        [StringLength(32)]
        public string icon { get; set; }
        public string description { get; set; }
        public bool is_redirect { get; set; } = false;
        public bool is_active { get; set; } = true;
        [IndexColumn("IX_Menu_ParentMenu_id", IsUnique = false)]
        public int parent_menu_id { get; set; } = 0;
    }

    public class SystemMenu_Filter
    {
        public int id { get; set; }
        [StringLength(100)]
        public string name { get; set; }
        public string code { get; set; }
        public string url { get; set; }
        [StringLength(32)]
        public string icon { get; set; }
        public string description { get; set; }
        public bool is_redirect { get; set; } = false;
        public bool is_active { get; set; } = true;
        public int parent_menu_id { get; set; } = 0;
    }
}
