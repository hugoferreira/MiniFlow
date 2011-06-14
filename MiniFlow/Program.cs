using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniFlow {    
    class Program {        
        static void Main(string[] args) {
 //           Scenario: Simple Transition
 //Given I have a simple process
 //When I initialize the process
 //And I execute the current task
 //Then I should still be executing the process
 //And the current node is 2
 //And there are 1 executing processes

            var t = new Core();
            t.SimpleProcess();
            t.InitializeProcess();
            t.ExecuteTask();
            t.CheckProcessIsRunning();

            Console.ReadLine();
        }
    }
}
