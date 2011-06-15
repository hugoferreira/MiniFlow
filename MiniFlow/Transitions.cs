using System.Collections.Generic;

namespace MiniFlow {
    public class SequenceFlow : IExecutable {
        public Node Source { get; private set; }
        public Node Target { get; private set; }

        public SequenceFlow(Node src, Node tgt) {
            this.Source = src;
            this.Target = tgt;

            this.Source.Outgoing.Add(this);
            this.Target.Incoming.Add(this);
        }

        public virtual IEnumerable<Token> Execute(Token exe) {
            exe.currentNode = Target;
            yield return exe;
        }
    }

    public class ConditionalFlow<T> : SequenceFlow {
        public T Condition { get; private set; }

        public ConditionalFlow(Node src, Node tgt, T condition): base(src, tgt) {
            this.Condition = condition;
        }
    }

    public class DefaultFlow : SequenceFlow {
        public DefaultFlow(Node src, Node tgt): base(src, tgt) { }
    }
}
