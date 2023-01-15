namespace Proxima.SelfEdu.FiniteState.Tests.ClassicWindowExample;

public class DefaultSetupTests
{
    private FiniteStateMachine<WindowState> _fsm;

    [SetUp]
    public void Setup()
    {
        _fsm = WindowExampleMachine.Build(null);
    }

    [Test]
    public void CallingHandler_WithNoTransition_DoesNotChangeState()
    {
        _fsm.Handle(CloseMessage.Instance);
        Assert.That(_fsm.CurrentState, Is.EqualTo(WindowState.Closed));
    }

    [Test]
    public void CallingHandler_WithTransition_ChangesState()
    {
        _fsm.Handle(OpenMessage.Instance);
        Assert.That(_fsm.CurrentState, Is.EqualTo(WindowState.Opened));
    }

    [Test]
    public void SubsequentHandleCalls_FlipStates_BackAndForth()
    {
        _fsm.Handle(OpenMessage.Instance);
        _fsm.Handle(CloseMessage.Instance);
        Assert.That(_fsm.CurrentState, Is.EqualTo(WindowState.Closed));
    }
}