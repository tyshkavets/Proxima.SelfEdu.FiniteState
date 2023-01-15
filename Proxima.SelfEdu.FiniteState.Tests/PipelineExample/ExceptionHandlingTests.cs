namespace Proxima.SelfEdu.FiniteState.Tests.PipelineExample;

public class ExceptionHandlingTests
{
    private FiniteStateMachine<PipelineStep> _fsm;
    private FiniteStateMachine<PipelineStep> _oppositeFsm;
    
    [SetUp]
    public void Setup()
    {
        _fsm = PipelineExampleMachine.Build(null);

        var oppositeOptions = new FiniteStateMachineOptions<PipelineStep>
        {
            ThrowIfDuplicateStatesAdded = false,
            ThrowIfTransitionAlreadyExists = false,
            ThrowIfHandleCalledOnFinishedMachine = true
        };

        _oppositeFsm = PipelineExampleMachine.Build(oppositeOptions);
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
}