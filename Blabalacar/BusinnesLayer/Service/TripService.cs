using BusinnesLayer.Interface;
using BusinnesLayer.Models.Entities.Trip;
using DataLayer.Models;
using DataLayer.Repository.Interface;

namespace BusinnesLayer.Service;

public class TripService:ITripService
{
    private readonly ITripRepository _tripRepository;

    public TripService(ITripRepository tripRepository)
    {
        _tripRepository = tripRepository;
    }

    public Task<IEnumerable<Trip?>> GetTrips(CancellationToken cancellationToken = default)
        => _tripRepository.GetAll(cancellationToken);

    public async Task<Trip> CreateTrip(User user, CreateTripBody createTripBody, CancellationToken cancellationToken = default)
    {
        var route = new Route(new Guid(), createTripBody.StartRoute, createTripBody.EndRoute);
        var trip = new Trip{Id = new Guid(), RouteId = route.Id, Route = route,
            DepartureAt = createTripBody.DepartureAt};;
        
        trip!.Route.Trips!.Add(trip);
        trip.UserTrips!.Add(user);

        await _tripRepository.Insert(trip,cancellationToken);

        return trip;
    }

    public async Task<Trip> GetById(Guid tripId, CancellationToken cancellationToken = default)
    {
        var trip = await _tripRepository.GetById(tripId, cancellationToken);
        if (trip == null)
            throw new Exception("Trip doesn't exist");

        return trip;
    }

    public bool DoesUserHasTrip(Trip trip, Guid userId)
    {
        if (trip.UserTrips!.Any(user => user.Id != userId))
            throw new Exception($"trip {trip.Id} isn't related to current user");

        return true;
    }

    public async Task DeleteTrip(Trip trip, CancellationToken cancellationToken = default)
    {
        if (trip == null)
            throw new Exception("Trip doesn't exist");
        
        await _tripRepository.Delete(trip, cancellationToken);
    }

    public async Task Save(CancellationToken cancellationToken = default)
        => await _tripRepository.Save(cancellationToken);

    public async Task<List<Trip>> FoundTripByStartAndEndPoint(FoundTripBody foundTripBody, CancellationToken cancellationToken = default)
    {
       return await _tripRepository.foundByStartAndEndPoints(foundTripBody.startRoute, foundTripBody.endRoute,
            cancellationToken);
    }
}