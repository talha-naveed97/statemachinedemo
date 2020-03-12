using Automatonymous;
using JobStateMachine.Contracts.JobCreateEvent;
using JobStateMachine.Contracts.JobRequestCommandContract;
using JobStateMachine.Contracts.JobRequestReceivedContract;
using System;

namespace JobStateMachine
{
    public class JobMachine: MassTransitStateMachine<MachineState>
    {
        public State Submitted { get; private set; }
        public State Processed { get; private set; }

        public Event<IJobRequestReceivedEvent> JobRequestReceived { get; private set; }
        public Event<IJobCreateEvent> JobCreated { get; private set; }
        public Event<IJobRequestCommand> CreateJobCommandReceived { get; private set; }


        public JobMachine()
        {
            InstanceState(x => x.CurrentState);

            Event(() => CreateJobCommandReceived, cc => cc
                      .CorrelateBy(state => state.JobId, context => context.Message.JobId)
                      .SelectId(context => Guid.NewGuid()));

            Event(() => JobRequestReceived, x => x.CorrelateById(context => context.Message.CorrelationId));
            Event(() => JobCreated, x => x.CorrelateById(context => context.Message.CorrelationId));


            During(Initial,
                When(CreateJobCommandReceived).Then(context =>
                {
                    context.Instance.JobId = context.Data.JobId;
                })
                .Publish(ctx => new JobRequestReceivedEvent(ctx.Instance))
                .TransitionTo(Submitted)
                .ThenAsync(context => Console.Out.WriteLineAsync(context.Instance.ToString()))
            );

            During(Submitted,
                When(JobRequestReceived)
                .TransitionTo(Processed)
                .ThenAsync(context => Console.Out.WriteLineAsync(context.Instance.ToString())));

            During(Processed,
                When(JobCreated).Then(context =>
                {
                    context.Instance.JobId = context.Data.JobId;
                })
                .Publish(ctx => new JobCreateEvent(ctx.Instance)).Finalize()
                .ThenAsync(context => Console.Out.WriteLineAsync(context.Instance.ToString()))
                );

            SetCompletedWhenFinalized();
        }

      

    }
}
