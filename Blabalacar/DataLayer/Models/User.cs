using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace DataLayer.Models;

public class User: IdentityUser<Guid>
{
    [Required]
    public string? Name { get; set; } = string.Empty;
    
    public DateTimeOffset UserCreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<Trip> UserTrips { get; set; } = new List<Trip>();
    public bool IsVerified { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTimeOffset RefreshTokenCreatedAt { get; set; }
    public DateTimeOffset RefreshTokenExpiresAt { get; set; }
}