using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketingSystemDAL.Entities;
using TicketingSystemDAL.EntityFramework;
using TicketingSystemDAL.Repositories;
using Xunit;

namespace TicketingSystemTests.DALTests
{
    public class RepositoryTests
    {

        public class TestDbContext : TicketingSystemDbContext
        {
            public TestDbContext(DbContextOptions<TicketingSystemDbContext> options)
                : base(options) { }

            /// <summary>
            /// Test DbSet with the test entities that will be used for unit testing of repository.
            /// </summary>
            public DbSet<TestEntity> TestEntities { get; set; }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                base.OnModelCreating(modelBuilder);
                modelBuilder.Entity<TestEntity>().HasKey(e => e.Id);
            }
        }

        public class TestEntity
        {
            public TestEntity() { }
            public int Id { get; set; }
            public string Name { get; set; }
        }

        private TestDbContext CreateContext(string dbName = null)
        {
            dbName = dbName ?? Guid.NewGuid().ToString();
            var options = new DbContextOptionsBuilder<TicketingSystemDbContext>()
                .UseInMemoryDatabase(dbName)
                .Options;

            return new TestDbContext(options);
        }

        #region GetAll Tests

        [Fact]
        public void GetAll_ReturnsAllEntities()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();

            using (var context = CreateContext(dbName))
            {
                context.Events.Add(new Event { Id = 1, Name = "Test1" });
                context.Events.Add(new Event { Id = 2, Name = "Test2" });
                context.Events.Add(new Event { Id = 3, Name = "Test3" });
                context.SaveChanges();
            }

            using (var context = CreateContext(dbName))
            {
                var repository = new Repository<Event>(context);

                // Act
                var result = repository.GetAll();

                // Assert
                Assert.Equal(3, result.Count());
                Assert.Contains(result, e => e.Id == 1);
                Assert.Contains(result, e => e.Id == 2);
                Assert.Contains(result, e => e.Id == 3);
            }
        }

        #endregion

        #region GetAllAsync Tests

        [Fact]
        public async Task GetAllAsync_ReturnsAllEntities()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();

            using (var context = CreateContext(dbName))
            {
                context.TestEntities.Add(new TestEntity { Id = 1, Name = "Test1" });
                context.TestEntities.Add(new TestEntity { Id = 2, Name = "Test2" });
                context.TestEntities.Add(new TestEntity { Id = 3, Name = "Test3" });
                context.SaveChanges();
            }

            using (var context = CreateContext(dbName))
            {
                var repository = new Repository<TestEntity>(context);

                // Act
                var result = await repository.GetAllAsync();

                // Assert
                Assert.Equal(3, result.Count());
                Assert.Contains(result, e => e.Id == 1 && e.Name == "Test1");
                Assert.Contains(result, e => e.Id == 2 && e.Name == "Test2");
                Assert.Contains(result, e => e.Id == 3 && e.Name == "Test3");
            }
        }

        #endregion

        #region GetById Tests

        [Fact]
        public void GetById_WithValidId_ReturnsEntity()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();

            using (var context = CreateContext(dbName))
            {
                context.TestEntities.Add(new TestEntity { Id = 1, Name = "Test1" });
                context.SaveChanges();
            }

            using (var context = CreateContext(dbName))
            {
                var repository = new Repository<TestEntity>(context);

                // Act
                var result = repository.GetById(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(1, result.Id);
                Assert.Equal("Test1", result.Name);
            }
        }

        [Fact]
        public void GetById_WithNullId_ThrowsArgumentNullException()
        {
            // Arrange
            using (var context = CreateContext())
            {
                var repository = new Repository<TestEntity>(context);

                // Act & Assert
                var exception = Assert.Throws<ArgumentNullException>(() => repository.GetById(null));
                Assert.Equal("id", exception.ParamName);
            }
        }

        #endregion

        #region GetByIdAsync Tests

        [Fact]
        public async Task GetByIdAsync_WithValidId_ReturnsEntity()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();

            using (var context = CreateContext(dbName))
            {
                context.TestEntities.Add(new TestEntity { Id = 1, Name = "Test1" });
                context.SaveChanges();
            }

            using (var context = CreateContext(dbName))
            {
                var repository = new Repository<TestEntity>(context);

                // Act
                var result = await repository.GetByIdAsync(1);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(1, result.Id);
                Assert.Equal("Test1", result.Name);
            }
        }

        [Fact]
        public async Task GetByIdAsync_WithNullId_ThrowsArgumentNullException()
        {
            // Arrange
            using (var context = CreateContext())
            {
                var repository = new Repository<TestEntity>(context);

                // Act & Assert
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => repository.GetByIdAsync(null));
                Assert.Equal("id", exception.ParamName);
            }
        }

        #endregion

        #region GetByIdsAsync Tests

        [Fact]
        public async Task GetByIdsAsync_WithValidIds_ReturnsEntities()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();

            using (var context = CreateContext(dbName))
            {
                context.TestEntities.Add(new TestEntity { Id = 1, Name = "Test1" });
                context.TestEntities.Add(new TestEntity { Id = 2, Name = "Test2" });
                context.TestEntities.Add(new TestEntity { Id = 3, Name = "Test3" });
                context.SaveChanges();
            }

            using (var context = CreateContext(dbName))
            {
                var repository = new Repository<TestEntity>(context);

                // Act
                var ids = new List<int> { 1, 3 };
                var result = await repository.GetByIdsAsync(ids);

                // Assert
                Assert.Equal(2, result.Count());
                Assert.Contains(result, e => e.Id == 1);
                Assert.Contains(result, e => e.Id == 3);
                Assert.DoesNotContain(result, e => e.Id == 2);
            }
        }

        #endregion

        #region Insert Tests

        [Fact]
        public void Insert_WithValidEntity_AddsEntityToDbSet()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            using (var context = CreateContext(dbName))
            {
                var repository = new Repository<TestEntity>(context);
                var entity = new TestEntity { Id = 1, Name = "Test1" };

                // Act
                repository.Insert(entity);
                context.SaveChanges();
            }

            // Assert
            using (var context = CreateContext(dbName))
            {
                var entity = context.TestEntities.Find(1);
                Assert.NotNull(entity);
                Assert.Equal(1, entity.Id);
                Assert.Equal("Test1", entity.Name);
            }
        }

        [Fact]
        public void Insert_WithNullEntity_ThrowsArgumentNullException()
        {
            // Arrange
            using (var context = CreateContext())
            {
                var repository = new Repository<TestEntity>(context);

                // Act & Assert
                var exception = Assert.Throws<ArgumentNullException>(() => repository.Insert(null));
                Assert.Equal("entity", exception.ParamName);
            }
        }

        #endregion

        #region InsertAsync Tests

        [Fact]
        public async Task InsertAsync_WithValidEntity_AddsEntityToDbSet()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();
            using (var context = CreateContext(dbName))
            {
                var repository = new Repository<TestEntity>(context);
                var entity = new TestEntity { Id = 1, Name = "Test1" };

                // Act
                await repository.InsertAsync(entity);
                await context.SaveChangesAsync();
            }

            // Assert
            using (var context = CreateContext(dbName))
            {
                var entity = await context.TestEntities.FindAsync(1);
                Assert.NotNull(entity);
                Assert.Equal(1, entity.Id);
                Assert.Equal("Test1", entity.Name);
            }
        }

        [Fact]
        public async Task InsertAsync_WithNullEntity_ThrowsArgumentNullException()
        {
            // Arrange
            using (var context = CreateContext())
            {
                var repository = new Repository<TestEntity>(context);

                // Act & Assert
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => repository.InsertAsync(null));
                Assert.Equal("entity", exception.ParamName);
            }
        }

        #endregion

        #region Update Tests

        [Fact]
        public void Update_WithValidEntity_UpdatesEntity()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();

            using (var context = CreateContext(dbName))
            {
                context.TestEntities.Add(new TestEntity { Id = 1, Name = "Original" });
                context.SaveChanges();
            }

            // Act
            using (var context = CreateContext(dbName))
            {
                var repository = new Repository<TestEntity>(context);
                var entity = new TestEntity { Id = 1, Name = "Updated" };
                repository.Update(entity);
                context.SaveChanges();
            }

            // Assert
            using (var context = CreateContext(dbName))
            {
                var entity = context.TestEntities.Find(1);
                Assert.NotNull(entity);
                Assert.Equal(1, entity.Id);
                Assert.Equal("Updated", entity.Name);
            }
        }

        [Fact]
        public void Update_WithNullEntity_ThrowsArgumentNullException()
        {
            // Arrange
            using (var context = CreateContext())
            {
                var repository = new Repository<TestEntity>(context);

                // Act & Assert
                var exception = Assert.Throws<ArgumentNullException>(() => repository.Update(null));
                Assert.Equal("entity", exception.ParamName);
            }
        }

        [Fact]
        public void Update_WithInvalidId_ThrowsArgumentException()
        {
            // Arrange
            using (var context = CreateContext())
            {
                var repository = new Repository<TestEntity>(context);
                var entity = new TestEntity { Id = 0, Name = "Test1" }; // Invalid ID

                // Act & Assert
                var exception = Assert.Throws<ArgumentException>(() => repository.Update(entity));
                Assert.Equal("entity", exception.ParamName);
            }
        }

        #endregion

        #region UpdateRange Tests

        [Fact]
        public void UpdateRange_WithValidEntities_UpdatesEntities()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();

            using (var context = CreateContext(dbName))
            {
                context.TestEntities.Add(new TestEntity { Id = 1, Name = "Original1" });
                context.TestEntities.Add(new TestEntity { Id = 2, Name = "Original2" });
                context.SaveChanges();
            }

            // Act
            using (var context = CreateContext(dbName))
            {
                var repository = new Repository<TestEntity>(context);
                var entities = new List<TestEntity>
                {
                    new TestEntity { Id = 1, Name = "Updated1" },
                    new TestEntity { Id = 2, Name = "Updated2" }
                };
                repository.UpdateRange(entities);
                context.SaveChanges();
            }

            // Assert
            using (var context = CreateContext(dbName))
            {
                var entity1 = context.TestEntities.Find(1);
                var entity2 = context.TestEntities.Find(2);

                Assert.NotNull(entity1);
                Assert.Equal("Updated1", entity1.Name);

                Assert.NotNull(entity2);
                Assert.Equal("Updated2", entity2.Name);
            }
        }

        [Fact]
        public void UpdateRange_WithNullEntities_ThrowsArgumentNullException()
        {
            // Arrange
            using (var context = CreateContext())
            {
                var repository = new Repository<TestEntity>(context);

                // Act & Assert
                var exception = Assert.Throws<ArgumentNullException>(() => repository.UpdateRange(null));
                Assert.Equal("entities", exception.ParamName);
            }
        }

        [Fact]
        public void UpdateRange_WithInvalidIdEntity_ThrowsArgumentException()
        {
            // Arrange
            using (var context = CreateContext())
            {
                var repository = new Repository<TestEntity>(context);
                var entities = new List<TestEntity>
                {
                    new TestEntity { Id = 1, Name = "Test1" },
                    new TestEntity { Id = 0, Name = "Test2" } // Invalid ID
                };

                // Act & Assert
                var exception = Assert.Throws<ArgumentException>(() => repository.UpdateRange(entities));
                Assert.Equal("entities", exception.ParamName);
            }
        }

        #endregion

        #region Delete Tests

        [Fact]
        public void Delete_WithValidId_DeletesEntity()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();

            using (var context = CreateContext(dbName))
            {
                context.TestEntities.Add(new TestEntity { Id = 1, Name = "Test1" });
                context.SaveChanges();
            }

            // Act
            using (var context = CreateContext(dbName))
            {
                var repository = new Repository<TestEntity>(context);
                repository.Delete(1);
                context.SaveChanges();
            }

            // Assert 
            using (var context = CreateContext(dbName))
            {
                var entity = context.TestEntities.Find(1);
                Assert.Null(entity); 
            }
        }

        [Fact]
        public void Delete_WithNullId_ThrowsArgumentNullException()
        {
            // Arrange
            using (var context = CreateContext())
            {
                var repository = new Repository<TestEntity>(context);

                // Act & Assert
                var exception = Assert.Throws<ArgumentNullException>(() => repository.Delete((object)null));
                Assert.Equal("id", exception.ParamName);
            }
        }

        [Fact]
        public void Delete_WithEntity_DeletesEntity()
        {
            // Arrange
            var dbName = Guid.NewGuid().ToString();

            using (var context = CreateContext(dbName))
            {
                context.TestEntities.Add(new TestEntity { Id = 1, Name = "Test1" });
                context.SaveChanges();
            }

            // Act
            using (var context = CreateContext(dbName))
            {
                var repository = new Repository<TestEntity>(context);
                var entity = context.TestEntities.Find(1);
                repository.Delete(entity);
                context.SaveChanges();
            }

            // Assert
            using (var context = CreateContext(dbName))
            {
                var entity = context.TestEntities.Find(1);
                Assert.Null(entity);
            }
        }

        [Fact]
        public void Delete_WithNullEntity_ThrowsArgumentNullException()
        {
            // Arrange
            using (var context = CreateContext())
            {
                var repository = new Repository<TestEntity>(context);

                // Act & Assert
                var exception = Assert.Throws<ArgumentNullException>(() => repository.Delete((TestEntity)null));
                Assert.Equal("entity", exception.ParamName);
            }
        }

        #endregion
    }
}
