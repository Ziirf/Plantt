using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantt.Domain.DTOs.Room.Request
{
    public class CreateRoomRequest
    {
        public required string Name { get; set; }
        public int SunlightLevel { get; set; }
        public required bool IsInside { get; set; }
    }
}
