using Benriya.Share.Models;
using Benriya.Share.Models.CoreTags;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Benriya.Modules.CMS.Share.Models
{
    [Table("cms_category_tags")]
    public class Category_Tags:Common_Model_Min
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        [ForeignKey("Category")]
        [IndexColumn("IX_CMS_Category_Tags_cate_id", IsUnique = false)]
        public int category_id { get; set; }
        [ForeignKey("Tags")]
        [IndexColumn("IX_CMS_Tags_tag_id", IsUnique = false)]
        public Guid tag_id { get; set; }
        public virtual Tags Tags { get; set; }
        public virtual Category Category { get; set; }
    }
}
