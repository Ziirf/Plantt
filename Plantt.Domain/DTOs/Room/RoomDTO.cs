using Plantt.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plantt.Domain.DTOs.Room
{
    public class RoomDTO
    {
        public int Id { get; set; }
        public required string Name { get; set; }
    }
}
