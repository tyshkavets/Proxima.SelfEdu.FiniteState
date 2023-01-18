namespace Proxima.SelfEdu.FiniteState.Tests.ClassicWindowExample;

public class StartingStateTests
{
    private FiniteStateMachine<WindowState> _fsm;

    [SetUp]
    public void Setup()
    {
        _fsm = new FiniteStateMachine<WindowState>();
    }

    [Test]
    public void HandlingEvent_WithoutStartingState_Throws()
    {
        _fsm.AddState(WindowState.Opened);
        _fsm.AddState(WindowState.Closed);

        Assert.Throws<FiniteStateMachineSetupException>(() => _fsm.Handle(OpenMessage.Instance));
    }

    [Test]
    public void AddingMultipleStartingStates_Throws()
    {
        _fsm.AddStartingState(WindowState.Closed);

        Assert.Throws<FiniteStateMachineSetupException>(() => _fsm.AddStartingState(WindowState.Opened));
    }
}