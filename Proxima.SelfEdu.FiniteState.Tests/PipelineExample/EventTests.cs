using Proxima.SelfEdu.FiniteState.Configuration;

namespace Proxima.SelfEdu.FiniteState.Tests.PipelineExample;

public class EventTests
{
    private FiniteStateMachine<PipelineStep> _fsm;
    private int onFinishCalls;
    
    [SetUp]
    public void Setup()
    {
        var eventHandler = new DefaultFiniteStateMachineEventHandler<PipelineStep>
        {
            OnEnteringFinalState = _ => onFinishCalls++,
        };

        onFinishCalls = 0;

        _fsm = PipelineExampleMachine.Build(default, eventHandler);
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