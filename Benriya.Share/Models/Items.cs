using System;
using System.Collections.Generic;
using System.Text;

namespace Benriya.Share.Models
{
    public class ItemUuidValue
    {
        public Guid id { get; set; }  
        public Guid ref_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

    public class ItemIntValue
    {
        public int id { get; set; }
        public int ref_id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}
