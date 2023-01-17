namespace Proxima.SelfEdu.FiniteState.Configuration;

public class DefaultFiniteStateMachineEventHandler<TState> : IFiniteStateMachineEventHandler<TState>
{
    public Action<IMessage, TState> OnTransition { get; init; } = (_, _) => { };
    public Action<TState> OnEnteringState { get; init; } = _ => { };
    public Action<TState> OnLeavingState { get; init; } = _ => { };
    public Action<TState> OnEnteringFinalState { get; init; } = _ => { };
    public Action<IMessage, TState> OnNoTransition { get; init; } = (_, _) => { };

    public DefaultFiniteStateMachineEventHandler()
    {
    }

    public DefaultFiniteStateMachineEventHandler(IFiniteStateMachineEventHandler<TState> eventHandler)
    {
        if (eventHandler == null)
        {
            return;
        }
        
        if (eventHandler.OnTransition != default)
        {
            OnTransition = eventHandler.OnTransition;
        }
        
        if (eventHandler.OnEnteringState != default)
        {
            OnEnteringState = eventHandler.OnEnteringState;
        }

        if (eventHandler.OnLeavingState != default)
        {
            OnLeavingState = eventHandler.OnLeavingState;
        }

        if (eventHandler.OnNoTransition != default)
        {
            OnNoTransition = eventHandler.OnNoTransition;
        }

        if (eventHandler.OnEnteringFinalState != default)
        {
            OnEnteringFinalState = eventHandler.OnEnteringFinalState;
        }
    }
}