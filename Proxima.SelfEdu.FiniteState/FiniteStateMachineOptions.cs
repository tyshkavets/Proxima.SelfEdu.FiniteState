namespace Proxima.SelfEdu.FiniteState;

public class FiniteStateMachineOptions<TState>
{
    /// <summary>
    /// Determines whether or not machine should throw an exception if a message arrives after machine
    /// has achieved one of the final states. Disabled by default.
    /// </summary>
    public bool ThrowIfHandleCalledOnFinishedMachine { get; set; } = false;

    /// <summary>
    /// Determines whether or not machine should throw an exception if a state-transition rule is added during setup
    /// that is contradicting another rule. If disabled, new rule will overwrite existing rule.
    /// Enabled (throws) by default.
    /// </summary>
    public bool ThrowIfTransitionAlreadyExists { get; set; } = true;

    /// <summary>
    /// Determines whether or not machine should throw an exception if a state added that is already added.
    /// Enabled by default.
    /// </summary>
    public bool ThrowIfDuplicateStatesAdded { get; set; } = true;

    public Action<IMessage, TState> OnTransition;

    public Action<TState> OnAchievedState;

    public Action<TState> OnAchievedFinalState;

    public Action<IMessage, TState> OnNoTransition;
}