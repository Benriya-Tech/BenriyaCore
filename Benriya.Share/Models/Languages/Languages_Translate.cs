using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Benriya.Share.Models.Languages
{
    [Table("languages_translate")]
    public class Languages_Translate
    { 
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IndexColumn("IX_Lang_Translate_id",IsUnique = true)]
        public Guid id { get; set; }
        [Required]
        [ForeignKey("Language")]
        [IndexColumn("IX_Lang_Trans_lange_id", IsUnique = false)]
        public int lange_id { get; set; }
        [StringLength(32)]
        [IndexColumn("IX_Lang_Trans_com_name",IsUnique = false)]
        public string com_name { get; set; }
        [StringLength(64)]
        [IndexColumn("IX_Lang_Trans_sub_com", IsUnique = false)]
        public string sub_com { get; set; }
        [IndexColumn("IX_Lang_Trans_com_id", IsUnique = false)]
        public int com_id { get; set; }
        [IndexColumn("IX_Lang_Trans_com_uuid", IsUnique = false)]
        public Guid com_uuid { get; set; }
        [IndexColumn("IX_Lang_Trans_com_uuid", IsUnique = false)]
        public string content { get; set; }
        public virtual Languages_Country Language { get; set; }
    }
}
