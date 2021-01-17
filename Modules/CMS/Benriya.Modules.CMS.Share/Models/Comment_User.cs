using Benriya.Share.Models.SystemUsers;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Benriya.Modules.CMS.Share.Models
{
    [Table("cms_comment_users")]
    public class Comment_Users: User_Common
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        public Guid comment_id { get; set; }

        public virtual Comments Comments { get; set; }

    }
}
