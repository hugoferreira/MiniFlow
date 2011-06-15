using System.Collections.Generic;
using System.Linq;
using NBehave.Narrator.Framework;
using NBehave.Spec.NUnit;
using System;

namespace MiniFlow.Test {
    [ActionSteps]
    public partial class Core {        
        private string transitionState;
        private ProcessInstance SingleInstance { get { return this.process.Instances.First(); } }

        [When("I initialize the process")]
        [When("I start the process")]
        public void InitializeProcess() {
            this.process.Create();
        }

        [When("I execute the current task")]
        [When("I execute again")]
        public void ExecuteTask() { ExecuteTask(1); }

        [When("I execute $times times")]
        public void ExecuteTask(int times) {
            for(int i = 0; i < times; i++) this.SingleInstance.Execute();
        }
         
        [When("I set the transition to $val")]
        public void SetTransitionState(string val) {
            this.transitionState = val;
        }

        [Then("the process is $state")]
        public void CheckProcessState(string state) {
            this.SingleInstance.State.ShouldEqual(Enum.Parse(typeof(ExecutionState), state, true));
        }

        [Then("current node is $id")]
        [Then("the current node is $id")]
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
            this.SingleInstance.Executions.Where(e => e.State == ExecutionState.Hold).Count().ShouldEqual(count);
        }
    }
}
