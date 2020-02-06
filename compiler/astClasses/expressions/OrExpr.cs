using ll.type;
using System;

namespace ll.AST
{
    public class OrExpr : BinOp {
        public OrExpr(IAST left, IAST right): base(left, right, "||", new BooleanType())
        {
            if(!(left.type is BooleanType) || !(right.type is BooleanType))
                throw new ArgumentException("Or operator only accepts bool values as operands");
        }
    }
}