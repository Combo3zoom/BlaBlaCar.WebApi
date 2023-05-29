using System.Security.Claims;
using BusinnesLayer.Interface;
using BusinnesLayer.Models;
using BusinnesLayer.Models.Entities.User;
using DataLayer.Models;
using DataLayer.Repository.Interface;
using Microsoft.AspNetCore.Http;

namespace BusinnesLayer.Service;

public class UserService:IUserService
{
    private readonly IUserRepository _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserService(IUserRepository userRepository, IHttpContextAccessor httpContextAccessor)
    {
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }
    
    public Task<IEnumerable<User?>> GetAll(CancellationToken cancellationToken=default)
        => _userRepository.GetAll(cancellationToken);
    
    public async Task<User> GetMe(Guid id, CancellationToken cancellationToken=default)
    {
        var user = await _userRepository.GetById(id, cancellationToken);
        if (user == null)
            throw new Exception("User doesn't exist");
        
        return user;
    }

    public Guid GetMyId()
        => new Guid(_httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public async Task<User> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetById(id, cancellationToken);
        if (user == null)
            throw new Exception("User doesn't exist");
        
        return user;
    }

    public async Task<User> GetByName(string name, CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetByName(name, cancellationToken);
        if (user == null)
            throw new Exception($"User {name} doesn't exist");

        return user;
    }

    public async Task DeleteByUser(User user, CancellationToken cancellationToken = default)
    {
        if (user == null)
            throw new Exception("User doesn't exist");
        
        await _userRepository.Delete(user, cancellationToken);
    }
    public void UpdateSelfUser(User changeUser, UpdateUserBody user)
    {
        changeUser.Name = user.Name;
    }
    
    public void AdminUpdateUser(User changeUser, AdminUpdateUserBody user)
    {
        user.IsVerification = user.IsVerification;
    }

    public async Task UpdateUserVerification(UpdateUserVerificationBody updateUserVerificationBody,
        CancellationToken cancellationToken = default)
    {
        var user = await _userRepository.GetById(updateUserVerificationBody.Id, cancellationToken);
        if (user == null)
            throw new Exception("User doesn't exist");
        
        user.IsVerified = updateUserVerificationBody.isVerificated;
        await _userRepository.Save(cancellationToken);
    }

    public async Task Save(CancellationToken cancellationToken = default) =>
        await _userRepository.Save(cancellationToken);

}