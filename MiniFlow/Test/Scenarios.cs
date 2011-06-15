using System.Collections.Generic;
using System.Linq;
using NBehave.Narrator.Framework;
using NBehave.Spec.NUnit;
using System;

namespace MiniFlow.Test {
    public partial class Core {
        private Process process;
        [Given("I have a simple process")]
        [Given("a simple process")]
        public void SimpleProcess() {
            var startNode = new StartNode("1. Submit claim");
            var n2 = new Activity("2. Middle");
            var endNode = new EndNode("3. Closed");
            var t12 = new SequenceFlow(startNode, n2);
            var t23 = new SequenceFlow(n2, endNode);

            this.process = new Process(startNode);
        }

        [Given("I have an exclusive gateway scenario")]
        [Given("an exclusive gateway scenario")]
        public void XorGwProcess() {
            var startNode = new StartNode("1. Submit claim");
            var xorGw = new ExclusiveGateway<string>("X. Decide", (e => transitionState));
            var n2 = new Activity("2. Alternative A");
            var n3 = new Activity("3. Alternative B");
            var n4 = new Activity("D. Default");
            var endNode = new EndNode("4. Closed");

            var t1gw = new SequenceFlow(startNode, xorGw);
            var tgw2 = new ConditionalFlow<string>(xorGw, n2, "a");
            var tgw3 = new ConditionalFlow<string>(xorGw, n3, "b");
            var tgw4 = new DefaultFlow(xorGw, n4);
            var t2End = new SequenceFlow(n2, endNode);
            var t3End = new SequenceFlow(n3, endNode);
            var t4End = new SequenceFlow(n4, endNode);

            this.process = new Process(startNode);
        }

        [Given("a paralel gateway scenario")]
        public void SplitGwProcess() {
            var startNode = new StartNode("1. Submit claim");
            var xorGw = new ParallelGateway("X. Split");
            var n2 = new Activity("2. Child A");
            var n3 = new Activity("3. Child B");
            var joinGw = new JoinGateway("X. Join");
            var endNode = new EndNode("4. Closed");

            var t1gw = new SequenceFlow(startNode, xorGw);
            var tgw2 = new SequenceFlow(xorGw, n2);
            var tgw3 = new SequenceFlow(xorGw, n3);
            var t2End = new SequenceFlow(n2, joinGw);
            var t3End = new SequenceFlow(n3, joinGw);
            var tJE = new SequenceFlow(joinGw, endNode);

            this.process = new Process(startNode);
        }

        [Given("a split conditional scenario")]
        public void SplitConditionalProcess() {
            var startNode = new StartNode("1. Submit claim");
            var xorGw = new SplitConditionalGateway<string>("X. Split", (e => transitionState));
            var n2 = new Activity("2. Child A");
            var n3 = new Activity("3. Child B");
            var n4 = new Activity("4. Child C");
            var joinGw = new JoinGateway("X. Join");
            var endNode = new EndNode("5. Closed");

            var t1gw = new SequenceFlow(startNode, xorGw);
            var tgw2 = new ConditionalFlow<string>(xorGw, n2, "a");
            var tgw3 = new ConditionalFlow<string>(xorGw, n3, "a");
            var tgw4 = new ConditionalFlow<string>(xorGw, n4, "b");
            var t2Join = new SequenceFlow(n2, joinGw);
            var t3Join = new SequenceFlow(n3, joinGw);
            var t4Join = new SequenceFlow(n4, joinGw);
            var tJE = new SequenceFlow(joinGw, endNode);

            this.process = new Process(startNode);
        }

    }
}
