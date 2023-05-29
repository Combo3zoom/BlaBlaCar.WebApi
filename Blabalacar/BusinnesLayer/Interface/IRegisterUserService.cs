using BusinnesLayer.Models;
using BusinnesLayer.Models.Entities.User;
using DataLayer.Models;

namespace BusinnesLayer.Interface;

public interface IRegisterUserService
{
    Task Logout();
    Task<User> Register(RegisterUserDto request);
    Task Login(User user, RegisterUserDto request, CancellationToken cancellationToken = default);
    Task<IList<string>> GetRoles(User user);
    
    public string GetId();
    // public void SetRefreshToken(User? user);
    // public string CreateAccessToken(User? registerUser);
    // public void SetCacheRefreshToken(Guid currentUserId, string currentRefreshToken);
    // public bool IsCorrectRefreshToken(User user, string refreshToken);
    // public Task<User> GetValueFromCache(Guid currentUserId, CancellationToken cancellationToke);
}