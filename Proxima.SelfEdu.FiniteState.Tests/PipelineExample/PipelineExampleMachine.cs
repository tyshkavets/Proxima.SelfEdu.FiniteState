using Microsoft.Extensions.Options;
using Proxima.SelfEdu.FiniteState.Configuration;

namespace Proxima.SelfEdu.FiniteState.Tests.PipelineExample;

public static class PipelineExampleMachine
{
    public static FiniteStateMachine<PipelineStep> Build(FiniteStateMachineOptions options,
        IFiniteStateMachineEventHandler<PipelineStep> eventHandler)
    {
        var wrappedOptions = Options.Create(options);
        var fsm = FiniteStateMachine<PipelineStep>.Create(wrappedOptions, eventHandler, builder =>
        {
            builder.AddStartingState(PipelineStep.Start);
            builder.AddState(PipelineStep.Middle);
            builder.AddFinalState(PipelineStep.Finish);
            builder.AddTransition<FirstOperationMessage>(PipelineStep.Start, PipelineStep.Middle);
            builder.AddTransition<SecondOperationMessage>(PipelineStep.Middle, PipelineStep.Finish);
            builder.AddTransition<RetryOperationMessage>(PipelineStep.Start, PipelineStep.Start);
            builder.AddTransition<RetryOperationMessage>(PipelineStep.Middle, PipelineStep.Middle);
        });

        return fsm;
    }
}

public record FirstOperationMessage : TestMessage<FirstOperationMessage>;

public record SecondOperationMessage : TestMessage<SecondOperationMessage>;

public record RetryOperationMessage : TestMessage<RetryOperationMessage>;

public enum PipelineStep { Start, Middle, Finish }