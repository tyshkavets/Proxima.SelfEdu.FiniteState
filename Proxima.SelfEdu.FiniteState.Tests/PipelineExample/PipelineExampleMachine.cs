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
            builder
                .AddStartingState(PipelineStep.Start)
                .AddState(PipelineStep.Middle)
                .AddFinalState(PipelineStep.Finish)
                .AddTransition<FirstOperationMessage>(PipelineStep.Start, PipelineStep.Middle)
                .AddTransition<SecondOperationMessage>(PipelineStep.Middle, PipelineStep.Finish)
                .AddTransition<RetryOperationMessage>(PipelineStep.Start, PipelineStep.Start)
                .AddTransition<RetryOperationMessage>(PipelineStep.Middle, PipelineStep.Middle);
        });

        return fsm;
    }
}

public record FirstOperationMessage : TestMessage<FirstOperationMessage>;

public record SecondOperationMessage : TestMessage<SecondOperationMessage>;

public record RetryOperationMessage : TestMessage<RetryOperationMessage>;

public enum PipelineStep { Start, Middle, Finish }