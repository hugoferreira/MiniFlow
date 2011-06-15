using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MiniFlow.Test;

namespace MiniFlow {    
    class Program {        
        static void Main(string[] args) {
            var t = new Core();
            t.SimpleProcess();
            t.InitializeProcess();
            t.ExecuteTask();
            t.CheckProcessState("running");

            Console.ReadLine();
        }
    }
}
