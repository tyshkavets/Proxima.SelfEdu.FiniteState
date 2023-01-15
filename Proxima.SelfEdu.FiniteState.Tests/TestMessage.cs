namespace Proxima.SelfEdu.FiniteState.Tests;

public record TestMessage<T> : IMessage where T : TestMessage<T>, new()
{
    private static readonly Lazy<T> LazyInstance = new(() => new T());

    public static T Instance => LazyInstance.Value;
}