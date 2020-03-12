using JobStateMachine.Contracts;
using JobStateMachine.Contracts.JobRequestCommandContract;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StartProcess
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var requestedJobId ="3";
            if (string.IsNullOrWhiteSpace(requestedJobId))
            {
                Console.WriteLine("Please enter any id!");
                Console.ReadLine();
            }
            var bus = BusConfigurator.ConfigureBus();
            var sendToUri = new Uri($"{"rabbitmq://localhost/"}{"saga.service"}");
            var endPoint = await bus.GetSendEndpoint(sendToUri);
            await endPoint.Send<IJobRequestCommand>(new JobRequestCommand
            {
                JobId = requestedJobId
            });
            Console.WriteLine("Message is sent!");
            Environment.Exit(0);
        }
    }

    class JobRequestCommand : IJobRequestCommand
    {
        public string JobId { get; set; }
    }
}
