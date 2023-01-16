namespace Proxima.SelfEdu.FiniteState.Configuration;

public class DefaultFiniteStateMachineEventHandler<TState> : IFiniteStateMachineEventHandler<TState>
{
    public Action<IMessage, TState> OnTransition { get; init; } = (_, _) => { };
    public Action<TState> OnAchievedState { get; init; } = _ => { };
    public Action<TState> OnAchievedFinalState { get; init; } = _ => { };
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
        
        if (eventHandler.OnAchievedState != default)
        {
            OnAchievedState = eventHandler.OnAchievedState;
        }

        if (eventHandler.OnNoTransition != default)
        {
            OnNoTransition = eventHandler.OnNoTransition;
        }

        if (eventHandler.OnAchievedFinalState != default)
        {
            OnAchievedFinalState = eventHandler.OnAchievedFinalState;
        }
    }
}