using System.ComponentModel.DataAnnotations.Schema;

namespace Plantt.Domain.Entities
{
    [Table("Account")]
    public class AccountEntity
    {
        public int Id { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public Guid PublicId { get; set; }

        public required string Username { get; set; }

        public required byte[] HashedPassword { get; set; }

        public required byte[] Salt { get; set; }

        public required int Iterations { get; set; }

        public required string Email { get; set; }
        //public ICollection<TokenFamilyEntity> TokenFamilies { get; set; } = new List<TokenFamilyEntity>();
    }
}
