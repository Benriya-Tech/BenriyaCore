using Benriya.Share.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Benriya.Modules.CMS.Share.Models
{
    [Table("cms_content_likes")]
    public class Content_Likes: Common_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public bool is_active { get; set; } = true;
        public Guid content_id { get; set; }        
        public virtual Contents Content { get; set; }
    }
}
