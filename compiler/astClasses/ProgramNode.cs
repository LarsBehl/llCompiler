using LL.Types;
using System.Collections.Generic;

namespace LL.AST
{
    public class ProgramNode : IAST
    {
        public Dictionary<string, FunctionDefinition> FunDefs { get; set; }
        public Dictionary<string, StructDefinition> StructDefs { get; set; }
        public Dictionary<string, ProgramNode> Dependencies { get; set; }
        public IAST CompositUnit { get; set; }
        public Dictionary<string, IAST> Env { get; set; }
        public string FileName { get; set; }

        public ProgramNode(
            string fileName,
            int line, int column
        ) : this(
            fileName,
            new Dictionary<string, FunctionDefinition>(),
            new Dictionary<string, StructDefinition>(),
            line,
            column
        )
        {

        }

        public ProgramNode(
            string fileName,
            Dictionary<string, FunctionDefinition> funDefs,
            Dictionary<string, StructDefinition> structDefs,
            int line,
            int column
        ) : base(
            new ProgramType(),
            line,
            column
        )
        {
            this.FunDefs = funDefs;
            this.StructDefs = structDefs;
            this.Dependencies = new Dictionary<string, ProgramNode>();
            this.CompositUnit = null;
            this.Env = new Dictionary<string, IAST>();
            this.FileName = fileName;
        }
    }
}