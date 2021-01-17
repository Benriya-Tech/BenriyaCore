using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Toolbelt.ComponentModel.DataAnnotations.Schema.V5;

namespace Benriya.Share.Models.FileStore
{
    [Table("filestore_files")]
    public class FileStore_Files : Common_Model
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [IndexColumn("IX_FileStore_Files_id", IsUnique = true)]
        public Guid id { get; set; }
        [StringLength(40)]
        [IndexColumn("IX_FileStore_Files_name", IsUnique = true)]
        public string name { get; set; }        
        [StringLength(32)]
        [IndexColumn("IX_FileStore_Files_module", IsUnique = false)]
        public string module { get; set; }
        [IndexColumn("IX_FileStore_Files_is_active", IsUnique = false)]
        public bool is_active { get; set; } = true;
        public bool is_public { get; set; }
        public string url { get; set; }
        [StringLength(128)]
        public string title { get; set; }
        [IndexColumn("IX_FileStore_Files_Type_id", IsUnique = false)]
        public int file_type_id { get; set; }
        public virtual FileStore_FileType FileType { get; set; }
        [IndexColumn("IX_FileStore_Documents_Files_model_id", IsUnique = false)]
        public int model_id { get; set; }
        [IndexColumn("IX_FileStore_Documents_Files_model_uuid", IsUnique = false)]
        public Guid model_uuid { get; set; }
        [IndexColumn("IX_FileStore_Documents_Files_checksum", IsUnique = false)]
        public string check_sum { get; set; }
    }
}
