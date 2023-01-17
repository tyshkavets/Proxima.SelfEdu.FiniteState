using Proxima.SelfEdu.FiniteState.Configuration;

namespace Proxima.SelfEdu.FiniteState.Tests.ClassicWindowExample;

public class EventTests
{
    private FiniteStateMachine<WindowState> _fsm;
    private int _onEnteredCalls;
    private int _onLeavingCalls;
    private int _onTransitionCalls;
    private int _onNoTransitionCalls;
    
    [SetUp]
    public void Setup()
    {
        var eventHandler = new DefaultFiniteStateMachineEventHandler<WindowState>
        {
            OnEnteringState = _ => _onEnteredCalls++,
            OnTransition = (_, _) => _onTransitionCalls++,
            OnNoTransition = (_, _) => _onNoTransitionCalls++,
            OnLeavingState = _ => _onLeavingCalls++
        };

        _onEnteredCalls = _onTransitionCalls = _onNoTransitionCalls = _onLeavingCalls = default;

        _fsm = WindowExampleMachine.Build(null, eventHandler);
    }
    
    [Test]
    public void OnEnteredState_IsCalled_WhenStateEntered()
    {
        _fsm.Handle(OpenMessage.Instance);
        _fsm.Handle(OpenMessage.Instance);

        Assert.Multiple(() =>
        {
            Assert.That(_onEnteredCalls, Is.EqualTo(1));
            Assert.That(_onLeavingCalls, Is.EqualTo(1));
        });
    }

    [Test]
    public void TransitionCallbacks_AreCalled()
    {
        _fsm.Handle(OpenMessage.Instance);
        _fsm.Handle(OpenMessage.Instance);
        _fsm.Handle(OpenMessage.Instance);
        _fsm.Handle(CloseMessage.Instance);
        _fsm.Handle(CloseMessage.Instance);

        Assert.Multiple(() =>
        {
            Assert.That(_onTransitionCalls, Is.EqualTo(2));
            Assert.That(_onNoTransitionCalls, Is.EqualTo(3));
        });
    }
}