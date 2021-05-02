using System;
using LL.Types;

namespace LL.AST
{
    public class DivAssignStatement : IAST
    {
        public VarExpr Left { get; set; }
        public IAST Right { get; set; }

        public DivAssignStatement(VarExpr left, IAST right, int line, int column) : base(new DivAssignStatementType(), line, column)
        {
            if (left.Type != right.Type)
            {
                if (left.Type is DoubleType && right.Type is IntType)
                {
                    this.Left = left;
                    this.Right = right;
                    return;
                }
                else
                    throw new ArgumentException($"Type of variable \"{left.Type.TypeName}\" does not match \"{right.Type.TypeName}\"; On line {line}:{column}");
            }

            this.Left = left;
            this.Right = right;
        }
    }
}