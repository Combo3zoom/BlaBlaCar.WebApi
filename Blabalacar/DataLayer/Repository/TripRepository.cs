using DataLayer.Models;
using DataLayer.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.Repository;

public class TripRepository:ITripRepository
{
    private readonly BlablacarContext _context;

    public TripRepository(BlablacarContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<Trip?>> GetAll(CancellationToken cancellationToken=default)
    {
        return await _context.Trips.Include(trip=>trip.UserTrips)
            .Include(trip=>trip.Route)
            .ToListAsync(cancellationToken: cancellationToken);
    }
    
    public async Task<Trip?> GetById(Guid id, CancellationToken cancellationToken=default)
    {
        return (await _context.Trips.Include(trip=>trip.UserTrips)
            .Include(trip=>trip.Route)
            .SingleOrDefaultAsync(trip => trip.Id == id, cancellationToken: cancellationToken))!;
    }

    public async Task Insert(Trip trip, CancellationToken cancellationToken=default)
    {
        await _context.Trips.AddAsync(trip, cancellationToken);
    }
    

    public async Task DeleteAt(Guid id, CancellationToken cancellationToken=default)
    {
        var trip = await _context.Trips.SingleOrDefaultAsync(currentTrip => currentTrip.Id == id,
            cancellationToken: cancellationToken);
        
        if (trip == null)
            return;
        _context.Trips.Remove(trip);
    }

    public Task Delete(Trip trip, CancellationToken cancellationToken=default)
    {
        _context.Trips.Remove(trip);
        return Task.CompletedTask;
    }

    public async Task Save(CancellationToken cancellationToken=default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Trip>> foundByStartAndEndPoints(string startRoute, string endRoute,
        CancellationToken cancellationToken = default)
    {
        return (await _context.Trips
            .Include(trip=>trip.Route)
            .Where(trip => trip.Route.StartRoute == startRoute && trip.Route.EndRoute == endRoute)
            .ToListAsync(cancellationToken));
    }
}