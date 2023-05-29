using BusinnesLayer.Models;
using BusinnesLayer.Models.Entities.Trip;
using DataLayer.Models;

namespace BusinnesLayer.Interface;

public interface ITripService
{
    Task<IEnumerable<Trip?>> GetTrips(CancellationToken cancellationToken = default);

    Task<Trip> CreateTrip(User user, CreateTripBody createTripBody, CancellationToken cancellationToken = default);
    Task<Trip> GetById(Guid tripId, CancellationToken cancellationToken=default);
    bool DoesUserHasTrip(Trip trip, Guid userId);
    Task DeleteTrip(Trip trip, CancellationToken cancellationToken = default);
    Task Save(CancellationToken cancellationToken = default);
    
    Task<List<Trip>> FoundTripByStartAndEndPoint(FoundTripBody foundTripBody, CancellationToken cancellationToken = default);
}