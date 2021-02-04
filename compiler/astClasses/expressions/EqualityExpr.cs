using System;
using ll.type;

namespace ll.AST
{
    public class EqualityExpr : BinOp
    {
        public EqualityExpr(IAST left, IAST right, int line, int column) : base(left, right, "==", new BooleanType(), line, column)
        {
            this.CheckType();
        }

        private void CheckType()
        {
            if(left.type != right.type)
            {
                if((left.type is not DoubleType || right.type is not IntType) && (left.type is not IntType || right.type is not DoubleType))
                    throw new ArgumentException($"Could not compare {this.left.type.typeName} with {this.right.type.typeName}; On line {this.line}:{this.column}");
            }
        }
    }
}