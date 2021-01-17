using Benriya.Share.Models;
using Benriya.Share.Models.SystemUsers;
using Benriya.Utils.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Benriya.Modules.CMS.Share.Models
{
    [Table("cms_contents")]
    public class Contents : Common_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        [StringLength(255)]
        public string name { get; set; }
        [StringLength(255)]
        public string description { get; set; }
        [StringLength(128)]
        public string path { get; set; }
        [DataType(DataType.MultilineText)]
        public string body { get; set; }
        public bool is_active { get; set; } = true;
        public int category_id { get; set; }
        [Required]
        public virtual Category Category { get; set; }
        [NotMapped]
        public virtual Users Users { get; set; }
        [NotMapped]
        public virtual ICollection<ItemUuidValue> Tags { get; set; }
        [NotMapped]
        public virtual DropdownItem InCategory { get; set; }
        public virtual ICollection<Comments> Comments { get; set; }
        public virtual ICollection<Content_Tags> Content_Tags { get; set; }
        public virtual ICollection<Content_Likes> Content_Likes { get; set; }
    }
}