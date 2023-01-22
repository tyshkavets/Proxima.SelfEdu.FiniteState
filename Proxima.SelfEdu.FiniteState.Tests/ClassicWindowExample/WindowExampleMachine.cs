using Microsoft.Extensions.Options;
using Proxima.SelfEdu.FiniteState.Configuration;

namespace Proxima.SelfEdu.FiniteState.Tests.ClassicWindowExample;

public static class WindowExampleMachine
{
    public static FiniteStateMachine<WindowState> Build(FiniteStateMachineOptions options,
        IFiniteStateMachineEventHandler<WindowState> eventHandler)
    {
        var wrappedOptions = Options.Create(options);
        var fsm = FiniteStateMachine<WindowState>.Create(wrappedOptions, eventHandler, builder =>
        {
            builder
                .AddStartingState(WindowState.Closed)
                .AddState(WindowState.Opened)
                .AddTransition<OpenMessage>(WindowState.Closed, WindowState.Opened)
                .AddTransition<CloseMessage>(WindowState.Opened, WindowState.Closed);
        });

        return fsm;
    }
}

public record OpenMessage : TestMessage<OpenMessage>;

public record CloseMessage : TestMessage<CloseMessage>;

public record SmashMessage : TestMessage<SmashMessage>;
    
public enum WindowState { Opened, Closed, Unknown };