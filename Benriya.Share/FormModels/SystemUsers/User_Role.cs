using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Benriya.Share.FormModels.SystemUsers
{
    public class User_Role_Form
    {
        public int id { get; set; }

        public string code { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public bool is_active { get; set; } = true;
    }
}
