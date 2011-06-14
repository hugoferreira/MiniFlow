using System.Collections.Generic;

namespace MiniFlow {
    public interface IExecutable {
        IEnumerable<Execution> Execute(Execution exe);
    }
}
