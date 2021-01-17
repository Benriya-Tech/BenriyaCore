using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Benriya.Share.Models.Helpers
{
    [Table("weight_type")]
    public class Weight_Type : Common_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IndexColumn("IX_Weight_Type_id", IsUnique = true)]
        public int id { get; set; }
        [StringLength(32)]
        [Required]
        [IndexColumn("IX_Weight_Type_name", IsUnique = true)]
        public string name { get; set; }
        [StringLength(6)]
        [Required]
        [IndexColumn("IX_Weight_Type_short_name", IsUnique = true)]
        public string short_name { get; set; }
        public string description { get; set; }
    }
}
