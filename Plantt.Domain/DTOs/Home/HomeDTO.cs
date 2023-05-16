using Plantt.Domain.DTOs.Room;
using Plantt.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantt.Domain.DTOs.Home
{
    public class HomeDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public List<RoomDTO>? Rooms { get; set; }
    }
}
