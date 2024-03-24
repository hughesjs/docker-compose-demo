namespace DemoWebApp.Common;

public interface IRepository<T>
{
    public Task<Option<DecoratedEntity<T>>> Get(string id);
    
    public Task<Option<DecoratedEntity<T>>> TrySet(T value);
}