namespace Proxima.SelfEdu.FiniteState;

public class FiniteStateMachine<TState>
{
    private readonly HashSet<TState> _states;
    private readonly HashSet<TState> _finalStates;
    private readonly IDictionary<(TState, Type), TState> _transitions;

    private bool _isFinished;
    private bool _startingStateSet;
    private TState _currentState;

    public FiniteStateMachine()
    {
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
            return;
        }

        var key = (_currentState, message.GetType());

        if (_transitions.ContainsKey(key))
        {
            _currentState = _transitions[key];

            if (_finalStates.Contains(_currentState))
            {
                _isFinished = true;
            }
        }
    }
    
    public void AddTransition<TMessage>(TState fromState, TState toState)
    {
        var key = (fromState, typeof(TMessage));

        if (_transitions.ContainsKey(key))
        {
            throw new FiniteStateMachineSetupException(
                $"Already registered transition from {fromState} on {typeof(TMessage).Name}");
        }

        if (!_states.Contains(fromState) || !_states.Contains(toState))
        {
            throw new FiniteStateMachineSetupException("Both ends of transition should be added first.");
        }
        
        _transitions.Add(key, toState);
    }

    public void AddState(TState state)
    {
        if (_states.Contains(state))
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
        else
        {
            AddState(state);
            _currentState = state;
            _startingStateSet = true;
        }
    }
}