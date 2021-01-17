using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Benriya.Share.Models.Languages
{
    [Table("languages_country")]
    public class Languages_Country : Common_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IndexColumn("IX_Lang_Country_id", IsUnique = true)]
        public int id { get; set; }
        [StringLength(32)]
        [IndexColumn("IX_Lang_Country_name", IsUnique = true)]
        public string name { get; set; }
        [StringLength(3)]
        [IndexColumn("IX_Lang_Country_code_3", IsUnique = true)]
        public string code_3 { get; set; }
        [StringLength(2)]
        [IndexColumn("IX_Lang_Country_code_2", IsUnique = true)]
        public string code_2 { get; set; }
        [StringLength(32)]
        [IndexColumn("IX_Lang_Country_lang_name", IsUnique = false)]
        public string lang_name { get; set; }
        [StringLength(32)]
        [IndexColumn("IX_Lang_Country_name_local", IsUnique = false)]
        public string name_local { get; set; }
    }
}
