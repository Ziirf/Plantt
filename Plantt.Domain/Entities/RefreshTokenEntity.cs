using Plantt.Domain.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace Plantt.Domain.Entities
{
    [Table("RefreshToken")]
    public class RefreshTokenEntity : IEntity
    {
        public int Id { get; set; }
        public required string Token { get; set; }
        public required bool Used { get; set; }
        public required DateTime IssuedTS { get; set; }
        public required DateTime ExpirationTS { get; set; }
        [ForeignKey("TokenFamily")]
        public int FK_TokenFamily_Id { get; set; }
        public required TokenFamilyEntity TokenFamily { get; set; }
    }
}
