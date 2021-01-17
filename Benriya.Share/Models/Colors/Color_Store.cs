using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Benriya.Share.Models.Colors
{
    [Table("color_store")]
    public class Color_Store : Common_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IndexColumn("IX_Color_Store_id", IsUnique = true)]
        public int id { get; set; }
        [StringLength(32)]        
        [Required]
        [IndexColumn("IX_Color_Store_name", IsUnique = true)]
        public string name { get; set; }
        [StringLength(7)]
        [Required]
        [IndexColumn("IX_Color_Store_hexcode", IsUnique = true)]
        public string hex_code { get; set; }

    }
}
