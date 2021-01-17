using System;
using System.ComponentModel.DataAnnotations;

namespace Benriya.Utils.Models
{
    public class CurrentUser
    {
        public bool is_loggedin { get; set; } = false;
        public Guid id { get; set; } = Guid.Empty;
        [StringLength(200)]
        public string name { get; set; }
        public string role { get; set; }
        [StringLength(32)]
        public string policy { get; set; }

    }
}
