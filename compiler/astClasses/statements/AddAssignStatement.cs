using System;
using ll.type;

namespace ll.AST
{
    public class AddAssignStatement : IAST
    {
        public VarExpr left { get; set; }
        public IAST right { get; set; }

        public AddAssignStatement(VarExpr left, IAST right, int line, int column) : base(new AddAssignStatementType(), line, column)
        {
            if (left.type != right.type)
            {
                if (left.type is DoubleType && right.type is IntType)
                {
                    this.left = left;
                    this.right = right;
                    return;
                }
                else
                    throw new ArgumentException($"Type of variable \"{left.type.typeName}\" does not match \"{right.type.typeName}\"; On line {line}:{column}");
            }

            this.left = left;
            this.right = right;
        }
    }
}