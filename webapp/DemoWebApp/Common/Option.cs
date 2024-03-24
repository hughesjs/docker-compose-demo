using System.Diagnostics.CodeAnalysis;

namespace DemoWebApp.Common;

public class Option<T>
{ 
    public bool HasSome { get; }
    
    [MemberNotNullWhen(returnValue: true, nameof(HasSome))]
    public T? Value { get; }

    public Option(T? value)
    {
        HasSome = value is not null;
        Value = value;
    }

    public Option()
    {
        HasSome = false;
        Value = default;
    }
}