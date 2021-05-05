using LL.Types;
using System.Collections.Generic;

namespace LL.AST
{
    public class ProgramNode : IAST
    {
        public Dictionary<string, FunctionDefinition> FunDefs { get; set; }
        public Dictionary<string, StructDefinition> StructDefs { get; set; }
        public Dictionary<string, ProgramNode> Dependencies { get; set; }

        public ProgramNode(int line, int column) : this(new Dictionary<string, FunctionDefinition>(), new Dictionary<string, StructDefinition>(), line, column)
        {

        }

        public ProgramNode(Dictionary<string, FunctionDefinition> funDefs, Dictionary<string, StructDefinition> structDefs, int line, int column) : base(new ProgramType(), line, column)
        {
            this.FunDefs = funDefs;
            this.StructDefs = structDefs;
            this.Dependencies = new Dictionary<string, ProgramNode>();
        }
    }
}