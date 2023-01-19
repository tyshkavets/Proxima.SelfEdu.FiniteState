using Microsoft.Extensions.Options;
using Proxima.SelfEdu.FiniteState.Configuration;

namespace Proxima.SelfEdu.FiniteState;

public class FiniteStateMachine<TState>
{
    private readonly FiniteStateMachineOptions _options;
    private readonly IFiniteStateMachineEventHandler<TState> _eventHandler;
    private readonly HashSet<TState> _states;
    private readonly HashSet<TState> _finalStates;
    private readonly IDictionary<(TState, Type), Func<IMessage, TState>> _transitions;

    private bool _isFinished;
    private bool _startingStateSet;
    private TState _currentState;

    public FiniteStateMachine() : this(default, default)
    {
    }

    public FiniteStateMachine(IOptions<FiniteStateMachineOptions> options,
        IFiniteStateMachineEventHandler<TState> eventHandler)
    {
        _options = options?.Value ?? new FiniteStateMachineOptions();
        _eventHandler = new DefaultFiniteStateMachineEventHandler<TState>(eventHandler);
        _states = new HashSet<TState>();
        _finalStates = new HashSet<TState>();
        _transitions = new Dictionary<(TState, Type), Func<IMessage, TState>>();
    }

    /// <summary>
    /// True if machine has entered one of the final states.
    /// </summary>
    public bool IsFinished => _isFinished;

    /// <summary>
    /// Current state the machine is in.
    /// </summary>
    public TState CurrentState
    {
        get => _currentState;
        private set
        {
            if (_currentState.Equals(value))
            {
                return;
            }

            _eventHandler.OnLeavingState(_currentState);
            _currentState = value;
            _eventHandler.OnEnteringState(_currentState);
            
            if (_finalStates.Contains(_currentState))
            {
                _isFinished = true;
                _eventHandler.OnEnteringFinalState(_currentState);
            }
        }
    }

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
        var _ = message ?? throw new ArgumentNullException(nameof(message));
        
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
            var proposedState = _transitions[key](message);

            if (!_states.Contains(proposedState))
            {
                throw new FiniteStateMachineOperationException(
                    $"Attempting transition to unknown state {proposedState}");
            }

            CurrentState = proposedState;
            _eventHandler.OnTransition(message, _currentState);
        }
        else
        {
            _eventHandler.OnNoTransition(message, _currentState);
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
    public void AddTransition<TMessage>(TState fromState, TState toState) where TMessage : IMessage
    {
        if (!_states.Contains(toState))
        {
            throw new FiniteStateMachineSetupException(
                $"Both ends of transition should be added first. Missing {toState}");
        }

        AddTransition<TMessage>(fromState, _ => toState);
    }

    public void AddTransition<TMessage>(TState fromState, Func<TMessage, TState> transitionRule) where TMessage : IMessage
    {
        var key = (fromState, typeof(TMessage));

        if (_transitions.ContainsKey(key) && _options.ThrowIfTransitionAlreadyExists)
        {
            throw new FiniteStateMachineSetupException(
                $"Already registered transition from {fromState} on {typeof(TMessage).Name}");
        }

        if (!_states.Contains(fromState))
        {
            throw new FiniteStateMachineSetupException(
                $"Both ends of transition should be added first. Missing {fromState}");
        }

        if (_finalStates.Contains(fromState))
        {
            throw new FiniteStateMachineSetupException("Cannot transition from final state.");
        }
        
        _transitions[key] = s => transitionRule((TMessage)s);
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