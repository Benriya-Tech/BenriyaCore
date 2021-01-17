using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Benriya.Share.Models.CoreTags
{
    [Table("tags_group")]
    public class Tags_Group : Common_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int id { get; set; }
        [Required]
        [StringLength(64, MinimumLength = 3)]
        public string name { get; set; }
        public string description { get; set; }
        public virtual ICollection<Tags> Tags { get; set; }
    }
}
