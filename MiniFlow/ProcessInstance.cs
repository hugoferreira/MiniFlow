using System.Collections.Generic;
using System.Linq;
using System;

namespace MiniFlow {
    public enum ExecutionState { Hold, Running, Closed }

    public class ProcessInstance {
        public IEnumerable<Token> Tokens { get; private set; }
        public Process Process { get; private set; }
        public Dictionary<DateTime, TimerNode> TimeEvents { get; private set; }

        public ProcessInstance(Process process) {
            this.Process = process;
            this.Tokens = new List<Token> { new Token(Process.StartNode) };
        }

        public void Execute() {
            this.Tokens = Tokens.SelectMany(e => e.Execute()).ToList();
        }

        public ExecutionState State {
            get {
                if (Tokens.Count() == 1 && Tokens.Single().State == ExecutionState.Closed) return ExecutionState.Closed;
                return ExecutionState.Running;
            }
        }
    }
}
