using Benriya.Share.Models;
using System;

namespace Benriya.Modules.CMS.Share.ViewModels
{
    public class Content_ViewModel: Common_Model_Min
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
        public int likes { get; set; }
        public int category_id { get; set; }
        public string category_name { get; set; }
    }

    public class CategoryMin
    {
        public int id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }

}
