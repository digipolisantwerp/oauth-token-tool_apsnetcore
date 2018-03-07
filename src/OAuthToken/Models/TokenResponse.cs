using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuthToken.Models
{
    public class TokenResponse
    {
        public string Token_type { get; set; }
        public string Access_token { get; set; }
        public string Expires_in { get; set; }
    }
}
