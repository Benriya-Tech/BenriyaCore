using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Benriya.Share.Models.SystemUsers
{
    [Table("user_direct_auth")]
    public class User_Directauth : Common_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        [StringLength(250)]
        public string key { get; set; }
        public DateTime expiry_date { get; set; }
        public Guid user_id { get; set; }
        public bool is_active { get; set; } = true;
        public virtual Users User { get; set; }
    }
}
