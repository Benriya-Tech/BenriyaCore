using Benriya.Share.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Benriya.Modules.CMS.Share.Models
{
    [Table("cms_comment_likes")]
    public class Comment_Likes: Common_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public bool is_active { get; set; } = true;
        public Guid comment_id { get; set; }
        public virtual Comments Comment { get; set; }
    }
}
