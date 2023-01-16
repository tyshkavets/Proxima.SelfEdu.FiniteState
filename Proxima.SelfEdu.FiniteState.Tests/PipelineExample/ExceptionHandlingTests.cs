using Proxima.SelfEdu.FiniteState.Configuration;

namespace Proxima.SelfEdu.FiniteState.Tests.PipelineExample;

public class ExceptionHandlingTests
{
    private FiniteStateMachine<PipelineStep> _fsm;
    private FiniteStateMachine<PipelineStep> _oppositeFsm;
    
    [SetUp]
    public void Setup()
    {
        _fsm = PipelineExampleMachine.Build(default, default);

        var oppositeOptions = new FiniteStateMachineOptions
        {
            ThrowIfDuplicateStatesAdded = false,
            ThrowIfTransitionAlreadyExists = false,
            ThrowIfHandleCalledOnFinishedMachine = true
        };

        _oppositeFsm = PipelineExampleMachine.Build(oppositeOptions, default);
    }

    [Test]
    public void ThrowFlag_ForFinishedState_ControlsException()
    {
        _fsm.Handle(FirstOperationMessage.Instance);
        _fsm.Handle(SecondOperationMessage.Instance);
        _oppositeFsm.Handle(FirstOperationMessage.Instance);
        _oppositeFsm.Handle(SecondOperationMessage.Instance);
        
        Assert.DoesNotThrow(() => _fsm.Handle(SecondOperationMessage.Instance));
        Assert.Throws<FiniteStateMachineOperationException>(() => _oppositeFsm.Handle(SecondOperationMessage.Instance));
    }

    [Test]
    public void Throws_WhenAddingTransition_FromFinalState()
    {
        Assert.Throws<FiniteStateMachineSetupException>(() =>
            _fsm.AddTransition<FirstOperationMessage>(PipelineStep.Finish, PipelineStep.Start));
    }
}