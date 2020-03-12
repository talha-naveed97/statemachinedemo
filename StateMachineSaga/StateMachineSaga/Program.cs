using JobStateMachine;
using JobStateMachine.Contracts;
using JobStateMachine.Contracts.JobCreateEvent;
using JobStateMachine.Contracts.JobRequestReceivedContract;
using MassTransit;
using MassTransit.Saga;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace StateMachineSaga
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            var sagaStateMachine = new JobMachine();
            var repository = new InMemorySagaRepository<MachineState>();
            var bus = BusConfigurator.ConfigureBus((cfg, host) =>
            {
                cfg.ReceiveEndpoint("saga.service", e =>
                {
                    e.StateMachineSaga(sagaStateMachine, repository);
                });

            });
            await bus.StartAsync(CancellationToken.None);
            Console.WriteLine("Saga active.. Press enter to exit");
            Console.ReadLine();
            await bus.StopAsync(CancellationToken.None);
        }
    }


}
