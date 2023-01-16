using Microsoft.Extensions.Options;

namespace Proxima.SelfEdu.FiniteState;

public class FiniteStateMachine<TState>
{
    private readonly FiniteStateMachineOptions<TState> _options;
    private readonly HashSet<TState> _states;
    private readonly HashSet<TState> _finalStates;
    private readonly IDictionary<(TState, Type), TState> _transitions;

    private bool _isFinished;
    private bool _startingStateSet;
    private TState _currentState;

    public FiniteStateMachine() : this(default)
    {
    }

    public FiniteStateMachine(IOptions<FiniteStateMachineOptions<TState>> options)
    {
        _options = options?.Value ?? new FiniteStateMachineOptions<TState>();
        _states = new HashSet<TState>();
        _finalStates = new HashSet<TState>();
        _transitions = new Dictionary<(TState, Type), TState>();
    }

    /// <summary>
    /// True if machine has achieved one of the final states.
    /// </summary>
    public bool IsFinished => _isFinished;

    /// <summary>
    /// Current state the machine is in.
    /// </summary>
    public TState CurrentState => _currentState;

    /// <summary>
    /// Processes a message received by the machine.
    /// </summary>
    /// <param name="message">Input message sent to the machine.</param>
    /// <exception cref="ArgumentNullException">
    /// Message must not be null.
    /// </exception>
    /// <exception cref="FiniteStateMachineSetupException">
    /// Machine is not set up properly and does not have initial state yet.
    /// </exception>
    /// <exception cref="FiniteStateMachineOperationException">
    /// Machine cannot process messages as it is already finished.
    /// </exception>
    public void Handle(IMessage message)
    {
        if (message == default)
        {
            throw new ArgumentNullException(nameof(message));
        }
        
        if (!_startingStateSet)
        {
            throw new FiniteStateMachineSetupException("Machine's initial state is unset.");
        }

        if (_isFinished)
        {
            // If machine has already finished, no transitions should occur and no messages handled.

            if (_options.ThrowIfHandleCalledOnFinishedMachine)
            {
                throw new FiniteStateMachineOperationException("Machine is finished");
            }
            
            return;
        }

        var key = (_currentState, message.GetType());

        if (_transitions.ContainsKey(key))
        {
            _currentState = _transitions[key];
            _options.OnTransition?.Invoke(message, _currentState);
            _options.OnAchievedState?.Invoke(_currentState);

            if (_finalStates.Contains(_currentState))
            {
                _isFinished = true;
                _options.OnAchievedFinalState?.Invoke(_currentState);
            }
        }
        else
        {
            _options.OnNoTransition?.Invoke(message, _currentState);
        }
    }
    
    /// <summary>
    /// Defines a state-transition rule. Machine in state fromState must transition
    /// to state toState when message of type TMessage is received.
    /// </summary>
    /// <param name="fromState">Machine state this rule is applicable for.</param>
    /// <param name="toState">State that machine ends up in if this rule is applied.</param>
    /// <typeparam name="TMessage">Type of message received.</typeparam>
    /// <exception cref="FiniteStateMachineSetupException">Thrown if rule cannot be setup.</exception>
    public void AddTransition<TMessage>(TState fromState, TState toState)
    {
        var key = (fromState, typeof(TMessage));

        if (_transitions.ContainsKey(key) && _options.ThrowIfTransitionAlreadyExists)
        {
            throw new FiniteStateMachineSetupException(
                $"Already registered transition from {fromState} on {typeof(TMessage).Name}");
        }

        if (!_states.Contains(fromState) || !_states.Contains(toState))
        {
            throw new FiniteStateMachineSetupException("Both ends of transition should be added first.");
        }

        if (_finalStates.Contains(fromState))
        {
            throw new FiniteStateMachineSetupException("Cannot transition from final state.");
        }
        
        _transitions[key] = toState;
    }

    /// <summary>
    /// Registers a state.
    /// </summary>
    /// <param name="state">Instance of a state. Must be unique.</param>
    /// <exception cref="FiniteStateMachineSetupException">
    /// Thrown if duplicate state is added and this exception is allowed in options. On by default.
    /// </exception>
    public void AddState(TState state)
    {
        if (_states.Contains(state) && _options.ThrowIfDuplicateStatesAdded)
        {
            throw new FiniteStateMachineSetupException($"State {state} has already been added.");
        }

        _states.Add(state);
    }

    /// <summary>
    /// Registers a state that is considered "final" for the machine. Machine can have any number of final states.
    /// Machine can have zero final states.
    /// </summary>
    /// <param name="state">Instance of a state. Must be unique.</param>
    public void AddFinalState(TState state)
    {
        AddState(state);
        _finalStates.Add(state);
    }

    /// <summary>
    /// Registers a state that machine is in upon creation. Machine must have exactly one starting state.
    /// Sending messages to machine without a starting state will cause an exception.
    /// </summary>
    /// <param name="state">Starting state of the machine.</param>
    /// <exception cref="FiniteStateMachineSetupException">Thrown if another starting set is already set.</exception>
    public void AddStartingState(TState state)
    {
        if (_startingStateSet)
        {
            throw new FiniteStateMachineSetupException("Cannot set multiple starting states");
        }

        AddState(state);
        _currentState = state;
        _startingStateSet = true;
    }
}