using Benriya.Share.Models;
using Benriya.Share.Models.CoreTags;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Benriya.Modules.CMS.Share.Models
{
    [Table("cms_content_tags")]
    public class Content_Tags :Common_Model_Min
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public Guid content_id { get; set; }
        [ForeignKey("Tags")]
        public Guid tag_id { get; set; }
        public virtual Tags Tags { get; set; }
        public virtual Contents Content { get; set; }
    }
}
