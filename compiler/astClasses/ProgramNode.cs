using LL.Types;
using System.Collections.Generic;

namespace LL.AST
{
    public class ProgramNode : IAST
    {
        public List<IAST> FunDefs { get; set; }
        public List<IAST> StructDefs { get; set; }

        public ProgramNode(List<IAST> funDefs, List<IAST> structDefs, int line, int column) : base(new ProgramType(), line, column)
        {
            this.FunDefs = funDefs;
            this.StructDefs = structDefs;
        }
    }
}