using System.ComponentModel.DataAnnotations;

namespace Benriya.Utils.Models
{
    public class CacheModel
    {
        public const string ApiKey = "ApiKey-";
        [StringLength(10)]
        public string Key { get; set; } = "ApiKey";
        public string Value { get; set; }
    }
}
