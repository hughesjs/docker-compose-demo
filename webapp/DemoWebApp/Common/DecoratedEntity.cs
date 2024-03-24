namespace DemoWebApp.Common;

public class DecoratedEntity<T>
{
    public static DecoratedEntity<T> Create(T value) => new()
    {
        Id = Guid.NewGuid(),
        CreatedAt = DateTime.UtcNow,
        Value = value
    };

    public required Guid Id { get; init; } 
    public required DateTime CreatedAt { get; init; }
    public required T Value { get; init; }
}