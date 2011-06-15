using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MiniFlow {
    public class Token {
        public Node currentNode { get; set; }
        public Token parentToken { get; private set; }
        public List<Token> childTokens { get; private set; }
        public static int counter = 0;
        public int id;

        public ExecutionState State {
            get {
                if (currentNode.GetType() == typeof(EndNode)) return ExecutionState.Closed;
                else if (this.childTokens.Count() > 0) return ExecutionState.Hold;
                return ExecutionState.Running;
            }
        }

        public Token(Token parent, Node startNode): this(startNode) {
            this.parentToken = parent;
        }

        public Token(Node startNode) {
            this.currentNode = startNode;
            this.childTokens = new List<Token>();
            this.id = counter++;
        }

        public IEnumerable<Token> Execute() {
            Console.WriteLine(this.ToString());

            if (this.State == ExecutionState.Hold) return new List<Token> { this };

            // TODO: Nao transitar e apresentar o estado como terminado
            if (parentToken != null && parentToken.childTokens.Count == 1) return parentToken.Execute();
            return currentNode.Execute(this).ToList();
        }

        public bool IsFinished { get { return State == ExecutionState.Closed; } }

        public Token Spawn() {
            var exe = new Token(this, currentNode);
            childTokens.Add(exe);
            return exe;
        }

        public Token Join() {
            parentToken.childTokens.Remove(this);
            return parentToken;
        }

        public override string ToString() {
            return "[" + id + "] Current Node is " + currentNode.Name;
        }
    }
}
