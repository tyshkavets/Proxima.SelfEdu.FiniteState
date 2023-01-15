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

    public bool IsFinished => _isFinished;

    public TState CurrentState => _currentState;

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

    public void AddState(TState state)
    {
        if (_states.Contains(state) && _options.ThrowIfDuplicateStatesAdded)
        {
            throw new FiniteStateMachineSetupException($"State {state} has already been added.");
        }

        _states.Add(state);
    }

    public void AddFinalState(TState state)
    {
        AddState(state);
        _finalStates.Add(state);
    }

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