using System.Collections.Generic;

namespace MiniFlow {
    public interface IExecutable {
        IEnumerable<Token> Execute(Token exe);
    }
}
