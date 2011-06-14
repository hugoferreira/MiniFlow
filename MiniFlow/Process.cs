using System.Collections.Generic;

namespace MiniFlow {
    public enum ProcessState { Running, Closed }

    public class Process {
        public StartNode StartNode { get; private set; }
        public List<ProcessInstance> Instances { get; private set; }

        public Process(StartNode startNode) {
            this.StartNode = startNode;
            this.Instances = new List<ProcessInstance>();
        }

        public ProcessInstance Create() {
            var i = new ProcessInstance(this);
            this.Instances.Add(i);
            return i;
        }
    }
}
