namespace Proxima.SelfEdu.FiniteState.Tests.ClassicWindowExample;

public class ExceptionHandlingTests
{
    private FiniteStateMachine<WindowState> _fsm;
    private FiniteStateMachine<WindowState> _oppositeFsm;

    [SetUp]
    public void Setup()
    {
        _fsm = WindowExampleMachine.Build(null);

        var oppositeOptions = new FiniteStateMachineOptions<WindowState>
        {
            ThrowIfDuplicateStatesAdded = false,
            ThrowIfTransitionAlreadyExists = false,
        };

        _oppositeFsm = WindowExampleMachine.Build(oppositeOptions);
    }

    [Test]
    public void Verify_AddingDuplicateStates_BehavesDifferently()
    {
        Assert.Throws<FiniteStateMachineSetupException>(() => _fsm.AddState(WindowState.Opened));
        Assert.DoesNotThrow(() => _oppositeFsm.AddState(WindowState.Opened));
    }

    [Test]
    public void Verify_AddingDuplicateTransitions_BehavesDifferently()
    {
        Assert.Throws<FiniteStateMachineSetupException>(() =>
            _fsm.AddTransition<OpenMessage>(WindowState.Closed, WindowState.Opened));
        
        Assert.DoesNotThrow(() =>
            _oppositeFsm.AddTransition<OpenMessage>(WindowState.Closed, WindowState.Opened));
    }
}