using System;
using ll.type;

namespace ll.AST
{
    public class DivAssignStatement : IAST
    {
        public VarExpr left { get; set; }
        public IAST right { get; set; }

        public DivAssignStatement(VarExpr left, IAST right, int line, int column) : base(new DivAssignStatementType(), line, column)
        {
            if (left.type.typeName != right.type.typeName)
            {
                if (left.type is DoubleType && right.type is IntType)
                {
                    this.left = left;
                    this.right = right;
                    return;
                }
                else
                    throw new ArgumentException($"Type of variable \"{left.type.typeName}\" does not match \"{right.type.typeName}\"");
            }

            this.left = left;
            this.right = right;
        }
    }
}