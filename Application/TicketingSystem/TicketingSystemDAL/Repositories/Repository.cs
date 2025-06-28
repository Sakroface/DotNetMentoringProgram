using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystemDAL.EntityFramework;
using TicketingSystemDAL.Repositories.Interfaces;

namespace TicketingSystemDAL.Repositories
{
    ///<inheritdoc/>
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        internal TicketingSystemDbContext Context;
        internal DbSet<TEntity> DbSet;

        public Repository(TicketingSystemDbContext context)
        {
            Context = context;
            DbSet = context.Set<TEntity>();
        }

        ///<inheritdoc/>
        public IEnumerable<TEntity> GetAll()
        {
            return DbSet;
        }

        ///<inheritdoc/>
        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        ///<inheritdoc/>
        public TEntity GetById(object id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id), "ID cannot be null.");

            return DbSet.Find(id);
        }

        ///<inheritdoc/>
        public virtual async Task<TEntity> GetByIdAsync(object id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id), "ID cannot be null.");

            return await DbSet.FindAsync(id);
        }


        ///<inheritdoc/>
        public async Task<IEnumerable<TEntity>> GetByIdsAsync<TKey>(IEnumerable<TKey> ids)
        {
            return await DbSet.Where(e => ids.Contains(EF.Property<TKey>(e, "Id"))).ToListAsync();
        }


        ///<inheritdoc/>
        public void Insert(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");

            DbSet.Add(entity);
        }

        ///<inheritdoc/>
        public async Task InsertAsync(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");

            await DbSet.AddAsync(entity);
        }

        ///<inheritdoc/>
        public void Update(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");

            // Check if entity has an ID property with a valid value
            var idProperty = typeof(TEntity).GetProperties()
                .FirstOrDefault(p => p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase));

            if (idProperty != null)
            {
                var idValue = idProperty.GetValue(entity);
                if (idValue == null ||
                    (idValue is int intId && intId <= 0) ||
                    (idValue is Guid guidId && guidId == Guid.Empty))
                {
                    throw new ArgumentException("Entity must have a valid ID.", nameof(entity));
                }
            }

            DbSet.Attach(entity);
            Context.Entry(entity).State = EntityState.Modified;
        }

        ///<inheritdoc/>
        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            if (entities == null)
                throw new ArgumentNullException(nameof(entities), "Entities collection cannot be null.");

            foreach (var entity in entities)
            {
                if (entity == null)
                    throw new ArgumentNullException(nameof(entity), "Entity in collection cannot be null.");

                var idProperty = typeof(TEntity).GetProperties()
                    .FirstOrDefault(p => p.Name.Equals("Id", StringComparison.OrdinalIgnoreCase));

                if (idProperty != null)
                {
                    var idValue = idProperty.GetValue(entity);
                    if (idValue == null ||
                        (idValue is int intId && intId <= 0) ||
                        (idValue is Guid guidId && guidId == Guid.Empty))
                    {
                        throw new ArgumentException($"Entity must have a valid ID. Invalid entity found in collection.", nameof(entities));
                    }
                }
            }

            DbSet.UpdateRange(entities);
        }

        ///<inheritdoc/>
        public void Delete(object id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id), "ID cannot be null.");

            TEntity entityToDelete = DbSet.Find(id);

            if (entityToDelete == null)
                return;

            Delete(entityToDelete);
        }

        ///<inheritdoc/>
        public void Delete(TEntity entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity), "Entity cannot be null.");

            if (Context.Entry(entity).State == EntityState.Detached)
            {
                DbSet.Attach(entity);
            }

            DbSet.Remove(entity);
        }
    }
}
