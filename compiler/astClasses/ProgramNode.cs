using ll.type;
using System.Collections.Generic;

namespace ll.AST
{
    public class ProgramNode : IAST
    {
        public List<IAST> funDefs { get; set; }
        public ProgramNode(List<IAST> funDefs): base(new ProgramType())
        {
            this.funDefs = funDefs;
        }
    }
}