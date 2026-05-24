namespace Infrastructure.Tests.Persistence.Common;

// Defines a collection so that only one instance of
// Postgres fixture gets created.
// All persistence test classes should use this collection
// Gets injected because we implemented `ICollectionFixture`
[CollectionDefinition("PostgresCollection")]
public class PostgresCollection : ICollectionFixture<PostgresFixture> {}