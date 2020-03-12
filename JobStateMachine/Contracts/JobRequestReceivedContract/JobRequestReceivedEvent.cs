using System;
using System.Collections.Generic;
using System.Text;

namespace JobStateMachine.Contracts.JobRequestReceivedContract
{
    public interface IJobRequestReceivedEvent
    {
        Guid CorrelationId { get; }
        string JobId { get; }
    }
   public class JobRequestReceivedEvent:IJobRequestReceivedEvent
    {
        private readonly MachineState machineState;

        public JobRequestReceivedEvent(MachineState machineState)
        {
            this.machineState = machineState;
        }

        public Guid CorrelationId => this.machineState.CorrelationId;

        public string JobId => this.machineState.JobId;

    }
}
