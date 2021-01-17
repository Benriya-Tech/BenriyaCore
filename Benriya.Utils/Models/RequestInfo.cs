using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Benriya.Utils.Models
{
    public class RequestInfo
    {
        public int id { get; set; }
        public Guid uuid { get; set; }
        public CurrentUser CurrentUser { get; set; }
        public ClientInfo Client { get; set; }   

    }

    public class ClientInfo
    {
        [StringLength(64)]
        public string id { get; set; }
        [StringLength(64)]
        public string key { get; set; }
        [StringLength(45)]
        public string ipAddress { get; set; }
        [StringLength(45)]
        public string ConnectionID { get; set; }
        public string UserAgent { get; set; }
        public string MacAddress { get; set; }
    }
}
