using LL.Types;
using System;

namespace LL.AST
{
    public class AssignStatement : IAST
    {
        public VarExpr Variable { get; set; }
        public IAST Value { get; set; }

        public AssignStatement(VarExpr variable, IAST value, int line, int column) : base(new AssignStatementType(), line, column)
        {
            if (variable.Type != value.Type)
            {
                if (variable.Type is DoubleType && value.Type is IntType)
                {
                    this.Variable = variable;
                    this.Value = value;
                    return;
                }
                else
                    throw new ArgumentException($"Variable type {value.Type} does not match {variable.Type}; On line {line}:{column}");

            }

            this.Variable = variable;
            this.Value = value;
        }
    }
}