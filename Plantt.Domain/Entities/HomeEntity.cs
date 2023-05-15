using Plantt.Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plantt.Domain.Entities
{
    [Table("Home")]
    public class HomeEntity : IEntity
    {
        public int Id { get; set; }
        public required string Name { get; set; }

        [ForeignKey(nameof(Account))]
        [Column("FK_Account_Id")]
        public int AccountId { get; set; }
        public required AccountEntity Account { get; set; }
    }
}
