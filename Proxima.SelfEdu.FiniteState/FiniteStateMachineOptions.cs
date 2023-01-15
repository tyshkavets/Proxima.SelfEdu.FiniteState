namespace Proxima.SelfEdu.FiniteState;

public class FiniteStateMachineOptions<TState>
{
    public bool ThrowIfHandleCalledOnFinishedMachine { get; set; } = false;

    public bool ThrowIfTransitionAlreadyExists { get; set; } = true;

    public bool ThrowIfDuplicateStatesAdded { get; set; } = true;

    public Action<IMessage, TState> OnTransition;

    public Action<TState> OnAchievedState;

    public Action<TState> OnAchievedFinalState;

    public Action<IMessage, TState> OnNoTransition;
}