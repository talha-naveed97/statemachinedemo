using System;
using System.Collections.Generic;
using System.Text;

namespace JobStateMachine.Contracts.JobCreateEvent
{
    public interface IJobCreateEvent
    {
        Guid CorrelationId { get; }
        string JobId { get; }
    }
    public class JobCreateEvent: IJobCreateEvent
    {
        private readonly MachineState machineState;

        public JobCreateEvent(MachineState machineState)
        {
            this.machineState = machineState;
        }

        public Guid CorrelationId => this.machineState.CorrelationId;

        public string JobId => this.machineState.JobId;
    }
}
