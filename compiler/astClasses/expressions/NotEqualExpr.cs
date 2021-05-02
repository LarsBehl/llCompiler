using LL.Types;
using System;

namespace LL.AST
{
    public class NotEqualExpr : BinOp
    {
        public NotEqualExpr(IAST left, IAST right, int line, int column) : base(left, right, "!=", new BooleanType(), line, column)
        {
            this.CheckType();
        }

        private void CheckType()
        {
            if(Left.Type != Right.Type)
            {
                if((Left.Type is not DoubleType || Right.Type is not IntType) && (Left.Type is not IntType || Right.Type is not DoubleType))
                    throw new ArgumentException($"Could not compare {this.Left.Type.typeName} with {this.Right.Type.typeName}; On line {this.Line}:{this.Column}");
            }
        }
    }
}