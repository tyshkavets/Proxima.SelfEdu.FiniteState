namespace Proxima.SelfEdu.FiniteState.Tests.PipelineExample;

public class DefaultSetupTests
{
    private FiniteStateMachine<PipelineStep> _fsm;

    [SetUp]
    public void Setup()
    {
        _fsm = PipelineExampleMachine.Build(default, default);
    }

    [Test]
    public void FinalState_IsTracked_Correctly()
    {
        _fsm.Handle(FirstOperationMessage.Instance);
        Assert.That(_fsm.IsFinished, Is.False);
        _fsm.Handle(SecondOperationMessage.Instance);
        Assert.That(_fsm.IsFinished, Is.True);
    }
}