using System.Collections.Generic;
using System.Threading.Tasks;

namespace TicketingSystemDAL.Repositories.Interfaces
{
    /// <summary>
    /// Generic repository interface for data access operations.
    /// </summary>
    /// <typeparam name="TEntity">The entity type this repository works with.</typeparam>
    public interface IRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Gets all entities from the repository.
        /// </summary>
        /// <returns>An IEnumerable of all entities.</returns>
        IEnumerable<TEntity> GetAll();

        /// <summary>
        /// Gets all entities from the repository asynchronously.
        /// </summary>
        /// <returns>A task that represents the asynchronous operation, containing all entities.</returns>
        Task<IEnumerable<TEntity>> GetAllAsync();

        /// <summary>
        /// Gets an entity by its ID
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>The entity if found, otherwise null.</returns>
        /// <exception cref="ArgumentNullException">Thrown when id is null.</exception>
        TEntity GetById(object id);

        /// <summary>
        /// Gets an entity by its ID asynchronously.
        /// </summary>
        /// <param name="id">The ID of the entity to retrieve.</param>
        /// <returns>A task that represents the asynchronous operation, containing the entity if found.</returns>
        /// <exception cref="ArgumentNullException">Thrown when id is null.</exception>
        Task<TEntity> GetByIdAsync(object id);

        /// <summary>
        /// Method to get entities range.
        /// </summary>
        /// <typeparam name="TKey">Supplies the type that the entity has.</typeparam>
        /// <param name="ids">Collection with the ids.</param>
        /// <returns>Collection with the entities.</returns>
        Task<IEnumerable<TEntity>> GetByIdsAsync<TKey>(IEnumerable<TKey> ids);

        /// <summary>
        /// Inserts a new entity into the repository.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <exception cref="ArgumentNullException">Thrown when entity is null.</exception>
        void Insert(TEntity entity);

        /// <summary>
        /// Inserts a new entity into the repository asynchronously.
        /// </summary>
        /// <param name="entity">The entity to insert.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        /// <exception cref="ArgumentNullException">Thrown when entity is null.</exception>
        Task InsertAsync(TEntity entity);

        /// <summary>
        /// Updates an existing entity in the repository.
        /// </summary>
        /// <param name="entity">The entity to update.</param>
        /// <exception cref="ArgumentNullException">Thrown when entity is null.</exception>
        /// <exception cref="ArgumentException">Thrown when entity has an invalid ID.</exception>
        void Update(TEntity entity);

        /// <summary>
        /// Method to update entries in the bulk.
        /// </summary>
        /// <param name="entities">Collection of entities to update.</param>
        void UpdateRange(IEnumerable<TEntity> entities);

        /// <summary>
        /// Deletes an entity from the repository by its Id.
        /// </summary>
        /// <param name="id">The Id of the entity to delete.</param>
        /// <exception cref="ArgumentNullException">Thrown when id is null.</exception>
        /// <exception cref="ArgumentException">Thrown when entity with the specified Id is not found.</exception>
        void Delete(object id);

        /// <summary>
        /// Deletes an entity from the repository.
        /// </summary>
        /// <param name="entity">The entity to delete.</param>
        /// <exception cref="ArgumentNullException">Thrown when entity is null.</exception>
        void Delete(TEntity entity);
    }

}
