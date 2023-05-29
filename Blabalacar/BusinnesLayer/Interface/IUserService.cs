using BusinnesLayer.Models;
using BusinnesLayer.Models.Entities.User;
using DataLayer.Models;

namespace BusinnesLayer.Interface;

public interface IUserService
{
    Task<IEnumerable<User?>> GetAll(CancellationToken cancellationToken = default);
    Task<User> GetMe(Guid id, CancellationToken cancellationToken = default);
    Guid GetMyId();
    Task<User> GetById(Guid id, CancellationToken cancellationToken = default);
    Task<User> GetByName(string name, CancellationToken cancellationToken = default);

    Task DeleteByUser(User user, CancellationToken cancellationToken = default);
    
    void UpdateSelfUser(User changeUser, UpdateUserBody user);
    void AdminUpdateUser(User changeUser, AdminUpdateUserBody user);

    Task UpdateUserVerification(UpdateUserVerificationBody updateUserVerificationBody,
        CancellationToken cancellationToken = default);

    Task Save(CancellationToken cancellationToken = default);

}