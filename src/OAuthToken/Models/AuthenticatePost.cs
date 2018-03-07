using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuthToken.Models
{
    public class AuthenticatePost
    {
        public string BaseUrl { get; set; } = "https://api-oauth2-a.antwerpen.be/v1/authorize";
        public string TokenUrl { get; set; } = "";
        public string ClientId { get; set; } = "";
        public string ClientSecret { get; set; } = "";
        public string CallbackUri { get; set; } = "http://localhost:50669/home/callback";
        public string Service { get; set; } = "astad.mprofiel.v1";
        public string Scope { get; set; } = "all";
    }
}
