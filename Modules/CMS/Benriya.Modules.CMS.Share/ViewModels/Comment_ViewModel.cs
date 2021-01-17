using Benriya.Share.Models;
using System;

namespace Benriya.Modules.CMS.Share.ViewModels
{
    public class Comment_ViewModel : Common_Model_View
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int likes { get; set; }
        public Guid content_id { get; set; }
        public string content_name { get; set; }
    }
}
