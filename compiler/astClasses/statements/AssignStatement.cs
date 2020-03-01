using ll.type;
using System;

namespace ll.AST
{
    public class AssignStatement : IAST
    {
        public VarExpr variable { get; set; }
        public IAST value { get; set; }

        // TODO check if var is struct that val has same struct name
        public AssignStatement(VarExpr variable, IAST value, int line, int column) : base(new AssignStatementType(), line, column)
        {
            if (variable.type.typeName != value.type.typeName)
            {
                if (variable.type is DoubleType && value.type is IntType
                || variable.type is RefType && value.type is NullType)
                {
                    this.variable = variable;
                    this.value = value;
                    return;
                }
                else
                    throw new ArgumentException($"Variable type {value.type} does not match {variable.type}; On line {line}:{column}");

            }
            this.variable = variable;
            this.value = value;
        }
    }
}