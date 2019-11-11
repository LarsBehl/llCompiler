using System.Collections.Generic;
using ll.type;

namespace ll.AST
{
    public class FunctionDefinition : IAST
    {
        public string name { get; set; }
        public List<string> args { get; set; }
        public IAST body { get; set; }

        public FunctionDefinition(string name, List<string> args, IAST body, type.Type type) : base(type)
        {
            this.name = name;
            this.args = args;
            this.body = body;
        }
    }
}