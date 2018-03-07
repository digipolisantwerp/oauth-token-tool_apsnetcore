using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuthToken.Models
{
    public class RequestInfoResponse
    {
        public IEnumerable<KeyValuePair<string, IEnumerable<string>>> HttpContext_Request_Headers { get; set; }
    }


}
