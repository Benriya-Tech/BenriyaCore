using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Benriya.Share.Models.FileStore
{
    public class FileStore_Model : Common_Model
    {
        public virtual Guid id { get; set; }
        public virtual string name { get; set; }
        public virtual string file_type { get; set; }
        public virtual string module { get; set; }
        public virtual bool is_active { get; set; }
        public virtual string url { get; set; }
        public int file_type_id { get; set; }
        public int model_id { get; set; }
        public Guid model_uuid { get; set; }
        public string check_sum { get; set; }
        //public string file_content { get; set; }
    }
}
