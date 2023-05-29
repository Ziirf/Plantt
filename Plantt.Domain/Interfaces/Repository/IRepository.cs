namespace Plantt.Domain.Interfaces.Repository
{
    public interface IRepository<TEntity> where TEntity : IEntity
    {
        /// <summary>
        /// Get a Entity by it's Id.
        /// </summary>
        /// <param name="id">Id of the Entity</param>
        /// <returns>Returns the Entity or null, if non were found.</returns>
        TEntity? GetById(int id);

        /// <summary>
        /// Get all Entities
        /// </summary>
        /// <returns>Returns all entities, as no tracking</returns>
        IEnumerable<TEntity> Getall();

        /// <summary>
        /// Add an entity to the database.
        /// </summary>
        /// <param name="entity">The entity which should be added to the database.</param>
        void Add(TEntity entity);

        /// <summary>
        /// Update an entity on the database.
        /// </summary>
        /// <param name="entity">The entity to be updated.</param>
        void Update(TEntity entity);

        /// <summary>
        /// Delete an entity from the database.
        /// </summary>
        /// <param name="entity">The entity to be deleted.</param>
        void Delete(TEntity entity);

        /// <summary>
        /// Find an entity on the database from Id async.
        /// </summary>
        /// <param name="id">The id of the entity.</param>
        /// <returns>Returns the async task, with the Entity if it could be found, otherwise it returns null.</returns>
        Task<TEntity?> GetByIdAsync(int id);

        /// <summary>
        /// Add an entity to the database async.
        /// </summary>
        /// <param name="entity">The entity to be added</param>
        /// <returns>Returns the async task.</returns>
        Task AddAsync(TEntity entity);


        /// <summary>
        /// Add a range of entity to the database async.
        /// </summary>
        /// <param name="entity">The entity to be added</param>
        /// <returns>Returns the async task.</returns>
        Task AddRangeAsync(IEnumerable<TEntity> entities);
    }
}
