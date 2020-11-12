# ef_ext_bulk_merge_error

```c#
var options = new DbContextOptionsBuilder<FooDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString("N"))
                .Options;
await using var dbContext = new FooDbContext(options) ;

//....

// The bulk insert works correctly as explained here: https://entityframework-extensions.net/efcore-inmemory-provider#create-database-context
await dbContext.Fooes.BulkInsertAsync(fooEntities, opt =>
{
    opt.IncludeGraph = true;
});

// However, the bulk merge doesn't work
// Exception details:: 'Oops! The 'IncludeGraph' feature doesn't work with 'BulkUpdate' and 'BulkMerge' NMemory provider.'
await dbContext.Fooes.BulkMergeAsync(fooEntities, opt =>
{
    opt.IncludeGraph = true;
});
```

Tested using:
- .Net Core 3
- Visual Studio 16.7.6
* The error occurs using the dotnet commands...
