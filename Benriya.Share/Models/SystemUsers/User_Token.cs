using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Benriya.Share.Models.SystemUsers
{
    [Table("user_tokens")]
    public class User_Token
    {
        public Guid id { get; set; }

        public string token { get; set; }
        public DateTime expiry { get; set; }
        public bool is_active { get; set; } = true;
        public Guid user_id { get; set; }

        public virtual Users User { get; set; }
    }
}
