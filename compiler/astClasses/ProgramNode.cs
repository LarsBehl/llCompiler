using ll.type;
using System.Collections.Generic;

namespace ll.AST
{
    public class ProgramNode : IAST
    {
        public List<IAST> funDefs { get; set; }
        public List<IAST> structDefs { get; set; }
        public ProgramNode(List<IAST> funDefs, List<IAST> structDefs) : base(new ProgramType())
        {
            this.funDefs = funDefs;
            this.structDefs = structDefs;
        }
    }
}