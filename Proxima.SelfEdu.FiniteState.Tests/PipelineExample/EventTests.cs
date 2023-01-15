namespace Proxima.SelfEdu.FiniteState.Tests.PipelineExample;

public class EventTests
{
    private FiniteStateMachine<PipelineStep> _fsm;
    private int onFinishCalls;
    
    [SetUp]
    public void Setup()
    {
        var options = new FiniteStateMachineOptions<PipelineStep>
        {
            OnAchievedFinalState = _ => onFinishCalls++,
        };

        onFinishCalls = 0;

        _fsm = PipelineExampleMachine.Build(options);
    }

    [Test]
    public void OnFinal_Called_WhenReached()
    {
        _fsm.Handle(FirstOperationMessage.Instance);
        Assert.That(onFinishCalls, Is.EqualTo(0));
        _fsm.Handle(SecondOperationMessage.Instance);
        Assert.That(onFinishCalls, Is.EqualTo(1));
    }
}