namespace DataLayer.Repository.Interface;

public interface IRepository<TModel,TId> where TModel:class
{
    Task<IEnumerable<TModel?>> GetAll(CancellationToken cancellationToken=default);
    Task<TModel?> GetById(TId? id, CancellationToken cancellationToken=default);
    Task Insert(TModel entity, CancellationToken cancellationToken=default);
    Task DeleteAt(TId? id, CancellationToken cancellationToken=default);
    Task Delete(TModel entity, CancellationToken cancellationToken=default);
    Task Save(CancellationToken cancellationToken=default);
}