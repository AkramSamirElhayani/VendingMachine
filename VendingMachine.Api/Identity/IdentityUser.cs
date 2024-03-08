using Microsoft.AspNetCore.Identity;

namespace VendingMachine.Api.Identity;

public class ApplicationUser : IdentityUser<Guid>
{
    public string RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; }
    public UserType UserType { get; set; }
    public Guid ActorId { get; set; }
}
