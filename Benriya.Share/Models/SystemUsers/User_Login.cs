using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Benriya.Share.Models.SystemUsers
{
    [Table("user_login_logs")]
    public class User_Login:User_Login_Common
    {
        public Guid id { get; set; }        

        public virtual Users User { get; set; }
    }

    public class User_Login_Common
    {
        public DateTime login_date { get; set; }
        public DateTime logout_update { get; set; }
        public DateTime logout_date { get; set; }
        public bool is_loggedIn { get; set; } = false;
        public Guid user_id { get; set; }
    }
}
