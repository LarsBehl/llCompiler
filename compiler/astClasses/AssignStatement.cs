using ll.type;
using System;

namespace ll.AST
{
    public class AssignStatement : IAST
    {
        public VarExpr variable { get; set; }
        public IAST value { get; set; }

        // TODO fix type check
        public AssignStatement(VarExpr variable, IAST value) : base(new AssignStatementType())
        {
            if (variable.type.typeName != value.type.typeName)
            {
                if (variable.type is DoubleType && value.type is IntType)
                {
                    this.variable = variable;
                    this.value = value;
                    return;
                }
                else
                    throw new ArgumentException($"Variable type {value.type} does not match {variable.type}");

            }
            this.variable = variable;
            this.value = value;
        }
    }
}