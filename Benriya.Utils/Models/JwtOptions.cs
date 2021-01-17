using System;
using System.Collections.Generic;
using System.Text;

namespace Benriya.Utils.Models
{
    public class JwtOptions
    {
        public const string Name = "JwtOptions";
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public bool ValidateIssuer { get; set; }
        public bool ValidateAudience { get; set; }
        public bool RequireHttps { get; set; }
        public string SecurityKey { get; set; } = "af8ce2c1-b27c-486f-9583-58f12d85342e";
    }
}
