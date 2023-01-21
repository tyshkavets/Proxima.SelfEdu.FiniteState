namespace Proxima.SelfEdu.FiniteState.Tests.ClassicWindowExample;

public class StartingStateTests
{
    [Test]
    public void HandlingEvent_WithoutStartingState_Throws()
    {
        var fsm = FiniteStateMachine<WindowState>.Create(builder =>
        {
            builder.AddState(WindowState.Opened);
            builder.AddState(WindowState.Closed);
        });

        Assert.Throws<FiniteStateMachineSetupException>(() => fsm.Handle(OpenMessage.Instance));
    }

    [Test]
    public void AddingMultipleStartingStates_Throws()
    {
        var _ = FiniteStateMachine<WindowState>.Create(builder =>
        {
            builder.AddStartingState(WindowState.Closed);
            Assert.Throws<FiniteStateMachineSetupException>(() => builder.AddStartingState(WindowState.Opened));
        });
    }
}