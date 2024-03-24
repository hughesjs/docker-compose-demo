namespace DemoWebApp.Common.Options;

public struct MongoOptions
{
    public required string ConnectionString { get; init; }
    public required string DatabaseName { get; init; }
}