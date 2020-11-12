using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Extensions;

namespace FooConsole
{
    internal static class Program
    {
        static async Task Main(string[] args)
        {
            var options = new DbContextOptionsBuilder<FooDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString("N"))
                .Options;

            await using (var dbContext = new FooDbContext(options))
            {
                var fooEntities = new[]
                {
                    new FooEntity{Name = Guid.NewGuid().ToString()},
                    new FooEntity{Name = Guid.NewGuid().ToString()},
                    new FooEntity{Name = Guid.NewGuid().ToString()}
                };

                EntityFrameworkManager.ContextFactory = context => dbContext;

                // The bulk insert works correctly as explained here: https://entityframework-extensions.net/efcore-inmemory-provider#create-database-context
                //await dbContext.Fooes.BulkInsertAsync(fooEntities, opt =>
                //{
                //    opt.IncludeGraph = true;
                //});

                // However, the bulk merge doesn't work
                // Exception details:: 'Oops! The 'IncludeGraph' feature doesn't work with 'BulkUpdate' and 'BulkMerge' NMemory provider.'
                await dbContext.Fooes.BulkMergeAsync(fooEntities, opt =>
                {
                    opt.IncludeGraph = true;
                });
            }

            await using (var dbContext = new FooDbContext(options))
            {
                var storedEntities = await dbContext.Fooes.ToListAsync();
                foreach (var entity in storedEntities)
                {
                    Console.WriteLine($"Entity Name: {entity.Name}");
                }
            }
        }

        public sealed class FooEntity
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public sealed class FooDbContext : DbContext
        {
            public DbSet<FooEntity> Fooes => Set<FooEntity>();

            public FooDbContext(DbContextOptions<FooDbContext> options) : base(options) { }
        }
    }
}
