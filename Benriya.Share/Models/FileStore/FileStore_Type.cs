using Benriya.Utils;
using ExtCore.Data.Entities.Abstractions;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Benriya.Share.Models.FileStore
{
    [Table("filestore_types")]
    public class FileStore_FileType : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IndexColumn("IX_FileStore_Type_id", IsUnique = true)]
        public int id { get; set; }
        [Required]
        [StringLength(32)]
        [IndexColumn("IX_FileStore_Type_name_extension", IsUnique = true)]
        public string file_extension { get; set; }
        [Required]
        [StringLength(32)]
        [IndexColumn("IX_FileStore_Type_name", IsUnique = false)]
        public string name { get; set; }
        [Required]
        [IndexColumn("IX_FileStore_Type_x", IsUnique = false)]
        public File_Types file_type { get; set; }
        public bool is_active { get; set; } = true;
        public virtual ICollection<FileStore_Files> Files { get; set; }
        public virtual ICollection<FileStore_Images> Images { get; set; }
        public virtual ICollection<FileStore_Documents> Documents { get; set; }
    }
}
