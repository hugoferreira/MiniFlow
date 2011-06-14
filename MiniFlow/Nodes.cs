using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniFlow {
    public class Node : IExecutable {
        public List<Transition> Arriving { get; private set; }
        public List<Transition> Leaving { get; private set; }
        public string Name { get; private set; }

        public Node(string name) {
            this.Arriving = new List<Transition>();
            this.Leaving = new List<Transition>();
            this.Name = name;
        }

        public virtual IEnumerable<Execution> Execute(Execution exe) {
            if (exe.parentExecution != null) Console.Write(" ");
            Console.WriteLine("[" + exe.id + "] Leaving " + this.Name);
            var t = this.Leaving.FirstOrDefault();
            if (t != null) return t.Execute(exe).ToList();
            return Enumerable.Empty<Execution>();
        }
    }

    public class StartNode : Node {
        public StartNode(string name) : base(name) { }
    }

    public class EndNode : Node {
        public EndNode(string name) : base(name) { }
    }

    public class SplitGateway : Node {
        public SplitGateway(string name) : base(name) { }

        public override IEnumerable<Execution> Execute(Execution exe) {
            foreach (var e in Leaving.SelectMany(t => t.Execute(exe.Spawn())))
                yield return e;
            yield return exe;
        }
    }

    public class JoinGateway : Node {
        public JoinGateway(string name) : base(name) { }

        public override IEnumerable<Execution> Execute(Execution exe) {
            if (exe.parentExecution.childs.All(c => c.currentNode == this)) {
                return base.Execute(exe.Join());
            } else
                return new List<Execution> { exe };
        }
    }

    public class ExclusiveGateway<T> : Node {
        public Func<Execution, T> EvaluationExpression { get; private set; }

        public ExclusiveGateway(string name, Func<Execution, T> evaluationExpression): base(name) {
            this.EvaluationExpression = evaluationExpression;
        }

        public override IEnumerable<Execution> Execute(Execution exe) {
            var result = EvaluationExpression(exe);
            foreach (var t in Leaving.Cast<Transition<T>>()) {
                if (t.Condition.Equals(result)) return t.Execute(exe);
            }

            return Enumerable.Empty<Execution>();
        }
    }

    public class SplitConditionalGateway<T> : ExclusiveGateway<T> {
        public SplitConditionalGateway(string name, Func<Execution, T> evaluationExpression) : base(name, evaluationExpression) { }

        public override IEnumerable<Execution> Execute(Execution exe) {
            var result = EvaluationExpression(exe);
            var exes = new List<Execution> { exe };
            exes.AddRange(Leaving.Cast<Transition<T>>()
                            .Where(t => t.Condition.Equals(result))
                            .SelectMany(e => e.Execute(exe.Spawn())));            

            return exes;
        }
    }

}
