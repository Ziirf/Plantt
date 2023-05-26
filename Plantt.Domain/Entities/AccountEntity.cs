using Plantt.Domain.Enums;
using Plantt.Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plantt.Domain.Entities
{
    [Table("Account")]
    public class AccountEntity : IEntity
    {
        public int Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Guid PublicId { get; set; }

        public required string Username { get; set; }

        [Column(TypeName = "varchar(200)")]
        public required string Email { get; set; }

        public AccountRoles Role { get; set; }

        public required byte[] HashedPassword { get; set; }

        public required byte[] Salt { get; set; }

        public required int Iterations { get; set; }

        public ICollection<HomeEntity>? Homes { get; set; }
    }
}
