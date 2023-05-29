using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using BusinnesLayer.Interface;
using BusinnesLayer.Models;
using BusinnesLayer.Models.Entities.User;
using DataLayer.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace BusinnesLayer.Service;

public class RegisterUserService:IRegisterUserService
{
    private readonly IMemoryCache _memoryCache;
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;

    public RegisterUserService(IMemoryCache memoryCache, IHttpContextAccessor httpContextAccessor, 
        IConfiguration configuration, UserManager<User> userManager, SignInManager<User> signInManager,
        IUserService userService)
    {
        _memoryCache = memoryCache;
        _httpContextAccessor = httpContextAccessor;
        _configuration = configuration;
        _userManager = userManager;
        _signInManager = signInManager;
        _userService = userService;
    }

    public async Task Logout() => await _signInManager.SignOutAsync();
    
    public async Task<User> Register(RegisterUserDto request)
    {
        var newUser = new User{Id=new Guid(), Name=request.Name, UserName = request.Name};
        var result = await _userManager.CreateAsync(newUser, request.Password);
        await _userManager.AddToRoleAsync(newUser, "Users");
        
        if (!result.Succeeded)
            throw new Exception($"{result.Errors}");

        await _signInManager.SignInAsync(newUser, isPersistent: false);
        
        return newUser;
    }

    public async Task Login(User user, RegisterUserDto request, CancellationToken cancellationToken = default)
    {
        var checkPassword = await _signInManager.PasswordSignInAsync(request.Name, request.Password, 
            false, false);
        if (!checkPassword.Succeeded)
            throw new Exception("Incorrect name or password");

        await _signInManager.CanSignInAsync(user);
    }

    public async Task<IList<string>> GetRoles(User user)=> await _userManager.GetRolesAsync(user);

    public string GetId()
    {
        var result = string.Empty;
        result = _httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier);
        return result;
    }
    // public void SetRefreshToken(User? user)
    // {
    //     user.RefreshToken = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));
    //     user.RefreshTokenCreatedAt = DateTime.Now;
    //     user.RefreshTokenExpiresAt = DateTime.Now.AddMinutes(300);
    // }
    //
    // public string CreateAccessToken(User? registerUser)
    // {
    //     var claims = new List<Claim>
    //     {
    //         new Claim(ClaimTypes.Name, registerUser.Name),
    //         new Claim(ClaimTypes.NameIdentifier, registerUser.Id.ToString())
    //     };
    //     var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
    //         _configuration.GetSection("JWT:Key").Value!));
    //     var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
    //     var token = new JwtSecurityToken(
    //         claims: claims,
    //         expires: DateTime.Now.AddMinutes(5),
    //         signingCredentials: creds);
    //     var jwt = new JwtSecurityTokenHandler().WriteToken(token);
    //     
    //     return jwt;
    // }
    //
    // public void SetCacheRefreshToken(Guid currentUserId, string currentRefreshToken)
    // {
    //     _memoryCache.Set(currentUserId, currentRefreshToken, new MemoryCacheEntryOptions
    //     {
    //         AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(20)
    //     });
    // }
    //
    // public bool IsCorrectRefreshToken(User user, string refreshToken)
    // {
    //     if (user.RefreshToken != refreshToken)
    //         throw new Exception("Incorrect refresh token");
    //
    //     return true;
    // }

    // public async Task<User> GetValueFromCache(Guid currentUserId, CancellationToken cancellationToken)
    // {
    //     User? currentUser;
    //     if (!_memoryCache.TryGetValue(currentUserId, out currentUser))
    //     {
    //         currentUser = await _userService.GetById(currentUserId, cancellationToken);
    //         if (currentUser == null)
    //             throw new Exception("User doesn't exist");
    //         
    //         SetCacheRefreshToken(currentUserId, currentUser.RefreshToken);
    //     }
    //
    //     return currentUser!;
    // }
}