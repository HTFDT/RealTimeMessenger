using System.ComponentModel.DataAnnotations;
using Core.Base.Dal;

namespace Dal.Entities;

public class RefreshTokenDal : BaseEntityDal<Guid>
{
    public required string RefreshToken { get; set; } = null!;
    [Required]
    public required DateTime? ExpiryDate { get; set; }
    
    public Guid UserId { get; set; }
    public UserDal User { get; set; } = null!;
}