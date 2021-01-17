using Benriya.Utils.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Benriya.Share.Models.CoreTags
{
    [Table("tags_data")]
    public class Tags : Common_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid id { get; set; }
        [Required]
        [StringLength(120, MinimumLength = 3)]
        public string name { get; set; }
        [StringLength(255)]
        public string description { get; set; }
        public int group_id { get; set; }
        [Required]
        [NotMapped]
        public virtual DropdownItem InGroup { get; set; }
        public virtual Tags_Group Group { get; set; }
    }
}
