using Plantt.Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plantt.Domain.Entities
{
    [Table("RefreshToken")]
    public class RefreshTokenEntity : IEntity
    {
        public int Id { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public required string Token { get; set; }
        public required bool Used { get; set; }
        public required DateTime IssuedTS { get; set; }
        public required DateTime ExpirationTS { get; set; }

        [ForeignKey(nameof(TokenFamily))]
        [Column("FK_TokenFamily_Id")]
        public int TokenFamilyId { get; set; }
        public required TokenFamilyEntity TokenFamily { get; set; }
    }
}
