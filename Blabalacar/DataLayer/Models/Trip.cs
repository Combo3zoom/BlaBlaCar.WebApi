using System.ComponentModel.DataAnnotations;

namespace DataLayer.Models;

public class Trip
{
    public Guid Id { get; set; }
    public Guid RouteId { get; set; }
    public Route Route { get; set; }
    public DateTime DepartureAt { get; set; }
    public DateTime TripCreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<User>? UserTrips { get; set; } = new List<User>();
}