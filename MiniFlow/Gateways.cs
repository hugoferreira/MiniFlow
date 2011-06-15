using System;
using System.Collections.Generic;
using System.Linq;

namespace MiniFlow {
    public class ParallelGateway : Node {
        public ParallelGateway(string name) : base(name) { }

        public override IEnumerable<Token> Execute(Token exe) {
            foreach (var e in Outgoing.SelectMany(t => t.Execute(exe.Spawn())))
                yield return e;
            yield return exe;
        }
    }

    public class JoinGateway : Node {
        public JoinGateway(string name) : base(name) { }

        public override IEnumerable<Token> Execute(Token exe) {
            if (exe.parentToken.childTokens.All(c => c.currentNode == this)) {
                return base.Execute(exe.Join());
            } else
                return new List<Token> { exe };
        }
    }

    public class ExclusiveGateway<T> : Node {
        public Func<Token, T> EvaluationExpression { get; private set; }

        public ExclusiveGateway(string name, Func<Token, T> evaluationExpression): base(name) {
            this.EvaluationExpression = evaluationExpression;
        }

        public override IEnumerable<Token> Execute(Token exe) {
            var result = EvaluationExpression(exe);
            foreach (var t in Outgoing.OfType<ConditionalFlow<T>>()) {
                if (t.Condition.Equals(result)) return t.Execute(exe);
            }

            var def = Outgoing.OfType<DefaultFlow>().Single();
            if (def != null) return def.Execute(exe);

            return Enumerable.Empty<Token>();
        }
    }

    public class SplitConditionalGateway<T> : ExclusiveGateway<T> {
        public SplitConditionalGateway(string name, Func<Token, T> evaluationExpression) : base(name, evaluationExpression) { }

        public override IEnumerable<Token> Execute(Token exe) {
            var result = EvaluationExpression(exe);
            var exes = new List<Token> { exe };
            exes.AddRange(Outgoing.Cast<ConditionalFlow<T>>()
                            .Where(t => t.Condition.Equals(result))
                            .SelectMany(e => e.Execute(exe.Spawn())));

            return exes;
        }
    }
}
