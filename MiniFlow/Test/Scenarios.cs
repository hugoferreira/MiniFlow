﻿using System.Collections.Generic;
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
            var t12 = new Transition(startNode, n2);
            var t23 = new Transition(n2, endNode);

            this.process = new Process(startNode);
        }

        [Given("I have an exclusive gateway scenario")]
        [Given("an exclusive gateway scenario")]
        public void XorGwProcess() {
            var startNode = new StartNode("1. Submit claim");
            var xorGw = new ExclusiveGateway<bool>("X. Decide", (e => transitionState));
            var n2 = new Activity("2. Alternative A");
            var n3 = new Activity("3. Alternative B");
            var endNode = new EndNode("4. Closed");

            var t1gw = new Transition(startNode, xorGw);
            var tgw2 = new Transition<bool>(xorGw, n2, true);
            var tgw3 = new Transition<bool>(xorGw, n3, false);
            var t2End = new Transition(n2, endNode);
            var t3End = new Transition(n3, endNode);

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

            var t1gw = new Transition(startNode, xorGw);
            var tgw2 = new Transition(xorGw, n2);
            var tgw3 = new Transition(xorGw, n3);
            var t2End = new Transition(n2, joinGw);
            var t3End = new Transition(n3, joinGw);
            var tJE = new Transition(joinGw, endNode);

            this.process = new Process(startNode);
        }

        [Given("a split conditional scenario")]
        public void SplitConditionalProcess() {
            var startNode = new StartNode("1. Submit claim");
            var xorGw = new SplitConditionalGateway<bool>("X. Split", (e => transitionState));
            var n2 = new Activity("2. Child A");
            var n3 = new Activity("3. Child B");
            var n4 = new Activity("4. Child C");
            var joinGw = new JoinGateway("X. Join");
            var endNode = new EndNode("5. Closed");

            var t1gw = new Transition(startNode, xorGw);
            var tgw2 = new Transition<bool>(xorGw, n2, true);
            var tgw3 = new Transition<bool>(xorGw, n3, true);
            var tgw4 = new Transition<bool>(xorGw, n4, false);
            var t2Join = new Transition(n2, joinGw);
            var t3Join = new Transition(n3, joinGw);
            var t4Join = new Transition(n4, joinGw);
            var tJE = new Transition(joinGw, endNode);

            this.process = new Process(startNode);
        }

    }
}