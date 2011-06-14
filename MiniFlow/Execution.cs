using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniFlow {
    public class Execution {
        public Node currentNode { get; set; }
        public Execution parentExecution { get; private set; }
        public List<Execution> childs { get; private set; }

        public ExecutionState state {
            get {
                if (currentNode.GetType() == typeof(EndNode)) return ExecutionState.Closed;
                else if (this.childs.Count() > 0) return ExecutionState.Hold;
                return ExecutionState.Running;
            }
        }

        public static int counter = 0;
        public int id;

        public Execution(Execution parent, Node startNode): this(startNode) {
            this.parentExecution = parent;
        }

        public Execution(Node startNode) {
            this.currentNode = startNode;
            this.childs = new List<Execution>();
            this.id = counter++;
        }

        public IEnumerable<Execution> Execute() {
            Console.WriteLine(this.ToString());

            if (this.state == ExecutionState.Hold) return new List<Execution> { this };

            // TODO: Nao transitar e apresentar o estado como terminado
            if (parentExecution != null && parentExecution.childs.Count == 1) return parentExecution.Execute();
            return currentNode.Execute(this).ToList();
        }

        public bool IsFinished { get { return state == ExecutionState.Closed; } }

        public Execution Spawn() {
            var exe = new Execution(this, currentNode);
            childs.Add(exe);
            return exe;
        }

        public Execution Join() {
            parentExecution.childs.Remove(this);
            return parentExecution;
        }

        public override string ToString() {
            return "[" + id + "] Current Node is " + currentNode.Name;
        }
    }

}
