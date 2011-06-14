using System.Collections.Generic;
using System.Linq;

namespace MiniFlow {
    public enum ExecutionState { Hold, Running, Closed }

    public class ProcessInstance {
        public IEnumerable<Execution> Executions { get; private set; }
        public Process Process { get; private set; }

        public ProcessInstance(Process process) {
            this.Process = process;
            this.Executions = new List<Execution> { new Execution(Process.StartNode) };
        }

        public void Execute() {
            this.Executions = Executions.SelectMany(e => e.Execute()).ToList();
        }

        public ExecutionState State {
            get {
                if (Executions.Count() == 1 && Executions.Single().state == ExecutionState.Closed) return ExecutionState.Closed;
                return ExecutionState.Running;
            }
        }
    }
}
