namespace Api.IntegrationTests.TestInfrastructure.Controllers;

// Defines a collection so that only one instance of
// Postgres fixture gets created.
// All persistence test classes should use this collection
// Gets injected because we implemented `ICollectionFixture`
[CollectionDefinition("WebAppCollection")]
public class WebAppCollection : ICollectionFixture<WebAppFactory> { }
