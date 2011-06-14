using System.Collections.Generic;
using System.Linq;
using NBehave.Narrator.Framework;
using NBehave.Spec.NUnit;

namespace MiniFlow {
    [ActionSteps]
    public class Core {
        private Process process;
        private bool transitionState;

        [Given("I have a simple process")]
        [Given("a simple process")]
        public void SimpleProcess() {
            var startNode = new StartNode("1. Submit claim");
            var n2 = new Node("2. Middle");
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
            var n2 = new Node("2. Alternative A");
            var n3 = new Node("3. Alternative B");
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
            var xorGw = new SplitGateway("X. Split");
            var n2 = new Node("2. Child A");
            var n3 = new Node("3. Child B");
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
            var n2 = new Node("2. Child A");
            var n3 = new Node("3. Child B");
            var n4 = new Node("4. Child C");
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

        private ProcessInstance SingleInstance { get { return this.process.Instances.First(); } }

        [When("I initialize the process")]
        [When("I start the process")]
        public void InitializeProcess() {
            this.process.Create();
        }

        [When("I execute the current task")]
        [When("I execute again")]
        public void ExecuteTask() {
            this.SingleInstance.Execute();
        }

        [When("I execute $times times")]
        public void ExecuteTask(int times) {
            for(int i = 0; i < times; i++) this.SingleInstance.Execute();
        }
         
        [When("I set the transition to $val")]
        public void SetTransitionState(bool val) {
            this.transitionState = val;
        }

        [Then("I should still be executing the process")]
        [Then("the process is still executing")]
        [Then("the process is not finished")]
        public void CheckProcessIsRunning() {
            this.SingleInstance.State.ShouldEqual(ExecutionState.Running);
        }

        [Then("the process is finished")]
        public void CheckProcessIsFinished() {
            this.SingleInstance.State.ShouldEqual(ExecutionState.Closed);
        }

        [Then("it is $state that the process is finished")]
        public void CheckProcessIsFinished(bool state) {
            (SingleInstance.State == ExecutionState.Closed).ShouldEqual(state);
        }

        [Then("current node is $id")]
        [Then("the current node is $id")]
        public void CheckCurrentNode(string id) {
            foreach (var e in SingleInstance.Executions) 
                e.currentNode.Name.ShouldStartWith(id);            
        }

        [Then("current node is $id")]
        [Then("the current nodes are $ids")]
        public void CheckCurrentNode(string[] ids) {
            var list = SingleInstance.Executions.Select(e => e.currentNode.Name.Substring(0, 1)).ToList();
            foreach (var id in ids) list.ShouldContain(id);
        }

        [Then("there are $count executing processes")]
        public void CountExecutions(int count) {
            this.SingleInstance.Executions.Count().ShouldEqual(count);
        }

        [Then("$count on hold")]
        public void CountExecutionsOnHold(int count) {
            this.SingleInstance.Executions.Where(e => e.state == ExecutionState.Hold).Count().ShouldEqual(count);
        }
    }
}
