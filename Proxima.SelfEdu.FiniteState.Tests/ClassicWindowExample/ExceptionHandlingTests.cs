using Proxima.SelfEdu.FiniteState.Configuration;

namespace Proxima.SelfEdu.FiniteState.Tests.ClassicWindowExample;

public class ExceptionHandlingTests
{
    private FiniteStateMachine<WindowState> _fsm;
    private FiniteStateMachine<WindowState> _oppositeFsm;

    [SetUp]
    public void Setup()
    {
        _fsm = WindowExampleMachine.Build(default, default);

        var oppositeOptions = new FiniteStateMachineOptions
        {
            ThrowIfDuplicateStatesAdded = false,
            ThrowIfTransitionAlreadyExists = false,
        };

        _oppositeFsm = WindowExampleMachine.Build(oppositeOptions, default);
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

    [Test]
    public void HandleCall_ForUnknownMessage_DoesNotThrow()
    {
        Assert.DoesNotThrow(() => _fsm.Handle(SmashMessage.Instance));
    }

    [Test]
    public void Transition_FromTheUnknownState_IsNotAllowed()
    {
        Assert.Throws<FiniteStateMachineSetupException>(() =>
            _fsm.AddTransition<OpenMessage>(WindowState.Unknown, WindowState.Opened));
    }

    [Test]
    public void Transition_ToTheUnknownState_Throws()
    {
        Assert.Throws<FiniteStateMachineSetupException>(() =>
            _fsm.AddTransition<OpenMessage>(WindowState.Opened, WindowState.Unknown));
    }
    
    [Test]
    public void Transition_ToTheUnknownState_ViaDelegate_Throws()
    {
        _fsm.AddTransition<OpenMessage>(WindowState.Opened, _ => WindowState.Unknown);
        
        _fsm.Handle(OpenMessage.Instance);
        
        Assert.Throws<FiniteStateMachineOperationException>(() =>
            _fsm.Handle(OpenMessage.Instance));
    }
}