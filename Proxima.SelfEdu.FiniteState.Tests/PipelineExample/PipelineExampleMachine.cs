using Microsoft.Extensions.Options;
using Proxima.SelfEdu.FiniteState.Configuration;

namespace Proxima.SelfEdu.FiniteState.Tests.PipelineExample;

public static class PipelineExampleMachine
{
    public static FiniteStateMachine<PipelineStep> Build(FiniteStateMachineOptions options,
        IFiniteStateMachineEventHandler<PipelineStep> eventHandler)
    {
        var wrappedOptions = Options.Create(options);
        var fsm = new FiniteStateMachine<PipelineStep>(wrappedOptions, eventHandler);
        fsm.AddStartingState(PipelineStep.Start);
        fsm.AddState(PipelineStep.Middle);
        fsm.AddFinalState(PipelineStep.Finish);
        fsm.AddTransition<FirstOperationMessage>(PipelineStep.Start, PipelineStep.Middle);
        fsm.AddTransition<SecondOperationMessage>(PipelineStep.Middle, PipelineStep.Finish);

        return fsm;
    }
}

public record FirstOperationMessage : TestMessage<FirstOperationMessage>;

public record SecondOperationMessage : TestMessage<SecondOperationMessage>;

public enum PipelineStep { Start, Middle, Finish }