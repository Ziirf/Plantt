using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantt.Domain.DTOs.Hub.Request
{
    public class LoginHubRequest
    {
        public required string Identity { get; set; }
        public required string Secret { get; set; }
    }
}
