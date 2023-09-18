namespace Application.Interfaces;
public interface IAsyncRepository<TEntity> where TEntity : class
{
    Task<IEnumerable<TEntity>> GetAll<TProperty>(List<Expression<Func<TEntity, TProperty>>> navProps);
    Task<TEntity> GetById<TId>(TId id);
    Task<IEnumerable<TEntity>> GetAll(Expression<Func<TEntity, bool>> predicate = null);
    Task<IEnumerable<TEntity>> GetAll();
    Task Add(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    Task<IEnumerable<TEntity>> WhereAsync(Expression<Func<TEntity, bool>> predicate);
    Task<IQueryable<TEntity>> WhereQueryable(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
}

