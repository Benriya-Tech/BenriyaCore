using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Benriya.Clients.Wasm.Components
{
    public static class CLIENT_CONFIG
    {
        public static string API_URL = @"https://localhost:5001/api/";
        public static string BASE_CLIENT_ADMIN  = @"/admin";
        public static string URL_CLIENT_ADMIN(string url)        {
            return BASE_CLIENT_ADMIN + url;
        }
        
    }

}
