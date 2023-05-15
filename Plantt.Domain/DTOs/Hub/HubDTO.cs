using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantt.Domain.DTOs.Hub
{
    public class HubDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string HomeName { get; set; }
        public required string Identity { get; set; }
        public required int HomeId { get; set; }
    }
}
