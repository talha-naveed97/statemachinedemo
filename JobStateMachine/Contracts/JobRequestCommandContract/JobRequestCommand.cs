using System;
using System.Collections.Generic;
using System.Text;

namespace JobStateMachine.Contracts.JobRequestCommandContract
{
    public interface IJobRequestCommand
    { 
        string JobId { get; }
    }
    public class JobRequestCommand: IJobRequestCommand
    {
        private readonly MachineState machineState;

        public JobRequestCommand(MachineState machineState)
        {
            this.machineState = machineState;
        }


        public string JobId => this.machineState.JobId;
    }
}
