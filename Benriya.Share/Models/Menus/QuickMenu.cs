using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Benriya.Share.Models.Menus
{
    [Table("menu_quick_access")]
    public class QuickMenu : Common_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public string description { get; set; }
        public bool is_active { get; set; } = true;
        [IndexColumn("IX_QuickMenu_on_SystemMenu_id",IsUnique = false)]
        [ForeignKey("SystemMenu")]
        public int system_menu_id { get; set; }
        public virtual SystemMenu SystemMenu { get; set; }
    }
}
