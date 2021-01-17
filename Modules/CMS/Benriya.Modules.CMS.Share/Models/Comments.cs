using Benriya.Share.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Benriya.Modules.CMS.Share.Models
{
    [Table("cms_comments")]
    public class Comments : Common_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        [DataType(DataType.MultilineText)]
        public string description { get; set; }
        public bool is_active { get; set; } = true;
        public Guid content_id { get; set; }
        public virtual Contents Contents { get; set; }
        public virtual ICollection<Comment_Users> Comment_Users { get; set; }
        public virtual ICollection<Comment_Likes> Comment_Likes{ get; set; }
    }
}
