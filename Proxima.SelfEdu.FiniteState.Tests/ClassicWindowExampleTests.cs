namespace Proxima.SelfEdu.FiniteState.Tests;

public class ClassicWindowExampleTests
{
    private record TestMessage<T> : IMessage where T : TestMessage<T>, new()
    {
        private static T Instance => new T();
    }
    private record OpenMessage : TestMessage<OpenMessage>;

    private record CloseMessage : TestMessage<CloseMessage>;

    private record SmashMessage : TestMessage<SmashMessage>;
    
    public enum WindowState { Opened, Closed };

    private FiniteStateMachine<WindowState> _fsm = default;

    [SetUp]
    public void Setup()
    {
        _fsm = new FiniteStateMachine<WindowState>();
        _fsm.AddStartingState(WindowState.Closed);
        _fsm.AddState(WindowState.Opened);
        _fsm.AddTransition<OpenMessage>(WindowState.Closed, WindowState.Opened);
        _fsm.AddTransition<CloseMessage>(WindowState.Opened, WindowState.Closed);
    }

    [Test]
    public void CallingHandler_WithNoTransition_DoesNotChangeState()
    {
        _fsm.Handle(new CloseMessage());
        Assert.That(_fsm.CurrentState, Is.EqualTo(WindowState.Closed));
    }

    [Test]
    public void CallingHandler_WithTransition_ChangesState()
    {
        _fsm.Handle(new OpenMessage());
        Assert.That(_fsm.CurrentState, Is.EqualTo(WindowState.Opened));
    }

    [Test]
    public void SubsequentHandleCalls_FlipStates_BackAndForth()
    {
        _fsm.Handle(new OpenMessage());
        _fsm.Handle(new CloseMessage());
        Assert.That(_fsm.CurrentState, Is.EqualTo(WindowState.Closed));
    }

    [Test]
    public void HandleCall_ForUnknownMessage_DoesNotThrow()
    {
        Assert.DoesNotThrow(() => _fsm.Handle(new SmashMessage()));
    }
}