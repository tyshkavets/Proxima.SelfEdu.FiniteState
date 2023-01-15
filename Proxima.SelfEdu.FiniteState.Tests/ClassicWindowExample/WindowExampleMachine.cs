using Microsoft.Extensions.Options;

namespace Proxima.SelfEdu.FiniteState.Tests.ClassicWindowExample;

public static class WindowExampleMachine
{
    public static FiniteStateMachine<WindowState> Build(FiniteStateMachineOptions<WindowState> options)
    {
        var wrappedOptions = Options.Create(options);
        var fsm = new FiniteStateMachine<WindowState>(wrappedOptions);
        fsm.AddStartingState(WindowState.Closed);
        fsm.AddState(WindowState.Opened);
        fsm.AddTransition<OpenMessage>(WindowState.Closed, WindowState.Opened);
        fsm.AddTransition<CloseMessage>(WindowState.Opened, WindowState.Closed);

        return fsm;
    }
}

public record TestMessage<T> : IMessage where T : TestMessage<T>, new()
{
    private static readonly Lazy<T> LazyInstance = new(() => new T());

    public static T Instance => LazyInstance.Value;
}

public record OpenMessage : TestMessage<OpenMessage>;

public record CloseMessage : TestMessage<CloseMessage>;

public record SmashMessage : TestMessage<SmashMessage>;
    
public enum WindowState { Opened, Closed };