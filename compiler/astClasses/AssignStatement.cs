using ll.type;
using System;

namespace ll.AST
{
    public class AssignStatement : IAST 
    {
        public VarExpr variable { get; set; }
        public IAST value { get; set; }

        public AssignStatement(VarExpr variable, IAST value) : base(new AssignStatementType())
        {
            if(variable.type != value.type)
                throw new ArgumentException($"Variable type {value.type} does not match {variable.type}");
            this.variable = variable;
            this.value = value;
        }
    }
}