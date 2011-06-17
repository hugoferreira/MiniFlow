using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace MiniFlow {
    public class Node : IExecutable {
        public List<SequenceFlow> Incoming { get; private set; }
        public List<SequenceFlow> Outgoing { get; private set; }
        public string Name { get; private set; }

        public Node(string name) {
            this.Incoming = new List<SequenceFlow>();
            this.Outgoing = new List<SequenceFlow>();
            this.Name = name;
        }

        public virtual IEnumerable<Token> Execute(Token exe) {
            if (exe.parentToken != null) Console.Write(" ");
            Console.WriteLine("[" + exe.id + "] Leaving " + this.Name);
            var t = this.Outgoing.FirstOrDefault();
            if (t != null) return t.Execute(exe).ToList();
            return Enumerable.Empty<Token>();
        }
    }

    public class Activity : Node {
        public Activity(string name) : base(name) { }

        public override IEnumerable<Token> Execute(Token exe) {
            if (Outgoing.Count() == 1) return base.Execute(exe);

            var p = new ParallelGateway("<Implicit>");
            p.Outgoing.AddRange(this.Outgoing);
            return p.Execute(exe);            
        }
    }

    public class StartNode : Node {
        public StartNode(string name) : base(name) { }
    }

    public class EndNode : Node {
        public EndNode(string name) : base(name) { }
    }

    public class TimerNode : Node {
        public TimeSpan TimeSpan { get; private set; }
        public Timer Timer { get; private set; }

        public TimerNode(string name, TimeSpan time): base(name) {
            this.TimeSpan = time;
        }

        public override IEnumerable<Token> Execute(Token exe) {
            this.Timer = new Timer(TimeSpan.Milliseconds);
            yield return exe;
        }
    }
}