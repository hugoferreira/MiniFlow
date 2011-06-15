using System.Collections.Generic;
using System.Linq;

namespace MiniFlow {
    public enum ExecutionState { Hold, Running, Closed }

    public class ProcessInstance {
        public IEnumerable<Token> Executions { get; private set; }
        public Process Process { get; private set; }

        public ProcessInstance(Process process) {
            this.Process = process;
            this.Executions = new List<Token> { new Token(Process.StartNode) };
        }

        public void Execute() {
            this.Executions = Executions.SelectMany(e => e.Execute()).ToList();
        }

        public ExecutionState State {
            get {
                if (Executions.Count() == 1 && Executions.Single().State == ExecutionState.Closed) return ExecutionState.Closed;
                return ExecutionState.Running;
            }
        }
    }
}
