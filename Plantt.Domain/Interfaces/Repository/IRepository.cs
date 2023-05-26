namespace Plantt.Domain.Interfaces.Repository
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        TEntity? GetById(int id);
        IEnumerable<TEntity> Getall();
        void Add(TEntity entity);
        void Update(TEntity entity);
        void Delete(TEntity entity);
        void Delete(int id);
        Task<TEntity?> GetByIdAsync(int id);
        Task AddAsync(TEntity entity);
    }
}
