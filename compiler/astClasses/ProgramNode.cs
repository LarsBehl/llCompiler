using LL.Types;
using System.Collections.Generic;

namespace LL.AST
{
    public class ProgramNode : IAST
    {
        public Dictionary<string, IAST> FunDefs { get; set; }
        public Dictionary<string, StructDefinition> StructDefs { get; set; }
        public Dictionary<string, ProgramNode> Dependencies { get; set; }

        public ProgramNode(int line, int column) : this(null, null, line, column)
        {

        }

        public ProgramNode(Dictionary<string, IAST> funDefs, Dictionary<string, StructDefinition> structDefs, int line, int column) : base(new ProgramType(), line, column)
        {
            this.FunDefs = funDefs;
            this.StructDefs = structDefs;
            this.Dependencies = null;
        }
    }
}