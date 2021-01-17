using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Benriya.Share.Models.SystemUsers
{
    [Table("user_credential")]
    public class User_Credential : Common_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }

        public byte[] password_hash { get; set; }
        public byte[] password_salt { get; set; }
        public Guid user_id { get; set; }

        public virtual Users User { get; set; }
    }
}
