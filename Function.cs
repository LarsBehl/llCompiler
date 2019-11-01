using System.Collections.Generic;

namespace ll
{
    public class Function : IAST
    {
        public string name { get; set; }
        public List<string> args { get; set; }
        public IAST body { get; private set; }

        public Function(string name, List<string> args, IAST body)
        {
            this.name = name;
            this.args = args;
            this.body = body;
        }
    }
}