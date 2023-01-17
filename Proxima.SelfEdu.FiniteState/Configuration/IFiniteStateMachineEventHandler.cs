namespace Proxima.SelfEdu.FiniteState.Configuration;

public interface IFiniteStateMachineEventHandler<in TState>
{
    /// <summary>
    /// Called upon successful transition. Accepts message causing a transition, and a state machine ended up in.
    /// </summary>
    public Action<IMessage, TState> OnTransition { get; }

    /// <summary>
    /// Called when machine enters a new state. Accepts that state as a parameter.
    /// </summary>
    public Action<TState> OnEnteringState { get; }
    
    /// <summary>
    /// Called when machine leaves a state. Accepts that state as a parameter.
    /// </summary>
    public Action<TState> OnLeavingState { get; }

    /// <summary>
    /// Called when machine enters a final state. Accepts that state as a parameter, as machine can have multiple
    /// final states that you may want to differentiate between in handling.
    /// Note that OnEnteringState is a separate event handler, and it, too, will be called when machine enters
    /// a final state. OnEnteringState will be called first.
    /// </summary>
    public Action<TState> OnEnteringFinalState { get; }

    /// <summary>
    /// Called when machine processes a message that does not cause a change of state.
    /// </summary>
    public Action<IMessage, TState> OnNoTransition { get; }
}