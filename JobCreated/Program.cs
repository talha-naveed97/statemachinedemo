using JobStateMachine.Contracts;
using JobStateMachine.Contracts.JobCreateEvent;
using MassTransit;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace JobCreated
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var bus = BusConfigurator.ConfigureBus((cfg, host) =>
            {
                cfg.ReceiveEndpoint("registerjob.service", e =>
                {
                    e.Consumer<JobCreatedConsumer>();
                });
            });

            await bus.StartAsync(CancellationToken.None);
            Console.WriteLine("Listening for report requests.. Press enter to exit");
            Console.ReadLine();
            await bus.StopAsync(CancellationToken.None);
        }
    }

    public class JobCreatedConsumer : IConsumer<IJobCreateEvent>
    {
        public async Task Consume(ConsumeContext<IJobCreateEvent> context)
        {
            var reportId = context.Message.JobId;
            await Console.Out.WriteLineAsync($"Report operation is succeeded! Report Id: {reportId}. Correlation Id: {context.Message.CorrelationId}");
            //Send mail, push notification, etc...

        }
    }
}
