using Automatonymous;
using System;
using System.Collections.Generic;
using System.Text;

namespace JobStateMachine
{
    public class MachineState : SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string JobId { get; set; }

        public State CurrentState { get; set; }
    }
}
