using System.Collections.Generic;

namespace MiniFlow {
    public class Transition : IExecutable {
        public Node Source { get; private set; }
        public Node Target { get; private set; }

        public Transition(Node src, Node tgt) {
            this.Source = src;
            this.Target = tgt;

            this.Source.Leaving.Add(this);
            this.Target.Arriving.Add(this);
        }

        public virtual IEnumerable<Execution> Execute(Execution exe) {
            exe.currentNode = Target;
            yield return exe;
        }
    }

    public class Transition<T> : Transition {
        public T Condition { get; private set; }

        public Transition(Node src, Node tgt, T condition)
            : base(src, tgt) {
            this.Condition = condition;
        }
    }
}
