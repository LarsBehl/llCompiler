using ll.type;
using System.Collections.Generic;

namespace ll.AST
{
    public class ProgramNode : IAST
    {
        public List<IAST> funDefs { get; set; }
        public List<IAST> structDefs { get; set; }
        public ProgramNode(List<IAST> funDefs, List<IAST> structDefs, int line, int column) : base(new ProgramType(), line, column)
        {
            this.funDefs = funDefs;
            this.structDefs = structDefs;
        }
    }
}