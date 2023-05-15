using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantt.Domain.DTOs.Hub
{
    public class HubWithSecretDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string HomeName { get; set; }
        public required string Identity { get; set; }
        public required string Secret { get; set; }
        public required int HomeId { get; set; }
    }
}
