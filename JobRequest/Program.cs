using JobStateMachine.Contracts;
using JobStateMachine.Contracts.JobCreateEvent;
using JobStateMachine.Contracts.JobRequestReceivedContract;
using MassTransit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JobRequest
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var bus = BusConfigurator.ConfigureBus((cfg, host) =>
            {
                cfg.ReceiveEndpoint("registerjob.service", e =>
                {
                    e.Consumer<JobRequestReceivedConsumer>();
                });
            });

            await bus.StartAsync(CancellationToken.None);
            Console.WriteLine("Listening for Job requests.. Press enter to exit");
            Console.ReadLine();
            await bus.StopAsync(CancellationToken.None);
        }
    }



    public class JobRequestReceivedConsumer : IConsumer<IJobRequestReceivedEvent>
    {
        public async Task Consume(ConsumeContext<IJobRequestReceivedEvent> context)
        {
            var jobId = context.Message.JobId;
            await Console.Out.WriteLineAsync($"Report request is received, report id is; {jobId}. Correlation Id: {context.Message.CorrelationId}");

            await context.Publish<IJobCreateEvent>(new
            {
                context.Message.CorrelationId,
                context.Message.JobId
            });
        }

    }
}
