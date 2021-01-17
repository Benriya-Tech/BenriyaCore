using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Benriya.Utils.Models
{
    public class DropdownItem
    {
        [StringLength(40)]
        public string value { get; set; }
        public int id { get; set; }
        public Guid uid { get; set; }
        public bool is_active { get; set; } = true;
        public string label { get; set; }
        public string ref_code { get; set; }
        public string description { get; set; }
    }
}
