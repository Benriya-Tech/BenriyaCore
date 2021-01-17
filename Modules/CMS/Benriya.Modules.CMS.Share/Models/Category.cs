using Benriya.Share.Models;
using Benriya.Share.Models.SystemUsers;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Benriya.Modules.CMS.Share.Models
{
    [Table("cms_category")]
    public class Category : Common_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [StringLength(255)]
        public string name { get; set; }
        [StringLength(255)]
        public string description { get; set; }
        public bool is_active { get; set; } = true;
        public virtual ICollection<Contents> Contents { get; set; }
        [NotMapped]
        public virtual ICollection<ItemUuidValue> Tags { get; set; }
        public virtual ICollection<Category_Tags> Category_Tags { get; set; }
        [NotMapped]
        public virtual Users Users { get; set; }
    }
}
