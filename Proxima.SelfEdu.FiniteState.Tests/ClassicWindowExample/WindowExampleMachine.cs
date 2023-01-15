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

public record OpenMessage : TestMessage<OpenMessage>;

public record CloseMessage : TestMessage<CloseMessage>;

public record SmashMessage : TestMessage<SmashMessage>;
    
public enum WindowState { Opened, Closed };