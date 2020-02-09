using ll.type;
using System;

namespace ll.AST
{
    public class OrExpr : BinOp
    {
        public OrExpr(IAST left, IAST right, int line, int column) : base(left, right, "||", new BooleanType(), line, column)
        {
            if (!(left.type is BooleanType) || !(right.type is BooleanType))
                throw new ArgumentException("Or operator only accepts bool values as operands");
        }
    }
}