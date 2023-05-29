using DataLayer.Models;

namespace DataLayer.Repository.Interface;

public interface ITripRepository:IRepository<Trip, Guid>
{
    Task<List<Trip>> foundByStartAndEndPoints(string startRoute, string endRoute, CancellationToken cancellationToken = default);
}