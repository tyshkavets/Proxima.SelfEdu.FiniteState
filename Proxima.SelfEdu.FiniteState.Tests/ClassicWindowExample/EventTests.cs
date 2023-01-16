using Proxima.SelfEdu.FiniteState.Configuration;

namespace Proxima.SelfEdu.FiniteState.Tests.ClassicWindowExample;

public class EventTests
{
    private FiniteStateMachine<WindowState> _fsm;
    private int onAchievedCalls;
    private int onTransitionCalls;
    private int onNoTransitionCalls;
    
    [SetUp]
    public void Setup()
    {
        var eventHandler = new DefaultFiniteStateMachineEventHandler<WindowState>
        {
            OnAchievedState = _ => onAchievedCalls++,
            OnTransition = (_, _) => onTransitionCalls++,
            OnNoTransition = (_, _) => onNoTransitionCalls++,
        };

        onAchievedCalls = onTransitionCalls = onNoTransitionCalls = default;

        _fsm = WindowExampleMachine.Build(null, eventHandler);
    }
    
    [Test]
    public void OnAchievedState_IsCalled_WhenStateAchieved()
    {
        _fsm.Handle(OpenMessage.Instance);
        
        Assert.That(onAchievedCalls, Is.EqualTo(1));
    }

    [Test]
    public void TransitionCallbacks_AreCalled()
    {
        _fsm.Handle(OpenMessage.Instance);
        _fsm.Handle(OpenMessage.Instance);
        _fsm.Handle(OpenMessage.Instance);
        _fsm.Handle(CloseMessage.Instance);
        _fsm.Handle(CloseMessage.Instance);
        
        Assert.That(onTransitionCalls, Is.EqualTo(2));
        Assert.That(onNoTransitionCalls, Is.EqualTo(3));
    }
}