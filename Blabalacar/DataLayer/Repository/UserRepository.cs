using DataLayer.Models;
using DataLayer.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repository;

public class UserRepository:IUserRepository
{
    private readonly BlablacarContext _context;

    public UserRepository(BlablacarContext context)
    {
        _context = context;
    }
    public async Task<User?> GetByName(string? name, CancellationToken cancellationToken=default)
    {
        return await _context.Users.Include(user => user.UserTrips)
            .FirstOrDefaultAsync(user => user.Name == name, cancellationToken);
    }

    public async Task<IEnumerable<User?>> GetAll(CancellationToken cancellationToken=default)
    {
        return await _context.Users.Include(user=>user.UserTrips)
            .ToListAsync(cancellationToken: cancellationToken);
    }

    public async Task<User?> GetById(Guid id, CancellationToken cancellationToken=default)
    {
        return (await _context.Users.Include(a=>a.UserTrips)
            .SingleOrDefaultAsync(user => user.Id == id, cancellationToken: cancellationToken))!;
    }

    public async Task Insert(User user, CancellationToken cancellationToken=default)
    {
        await _context.Users.AddAsync(user, cancellationToken);
    }

    public async Task DeleteAt(Guid id, CancellationToken cancellationToken=default)
    {
        var user = await _context.Users.
            FirstOrDefaultAsync(currentUser => currentUser.Id == id, cancellationToken: cancellationToken);
        if (user != null)
            _context.Users.Remove(user);
    }

    public Task Delete(User user, CancellationToken cancellationToken=default)
    {
        _context.Users.Remove(user);
        return Task.CompletedTask;
    }

    public async Task Save(CancellationToken cancellationToken=default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}