using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OAuthToken.Models
{
    public class CallbackQuery
    {
        public string Code { get; set; }
        public string State { get; set; }
    }
}
