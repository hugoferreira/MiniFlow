using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniFlow {    
    class Program {        
        static void Main(string[] args) {
            var t = new Core();
            t.SimpleProcess();
            t.InitializeProcess();
            t.ExecuteTask();
            t.CheckProcessIsFinished("running");

            Console.ReadLine();
        }
    }
}
