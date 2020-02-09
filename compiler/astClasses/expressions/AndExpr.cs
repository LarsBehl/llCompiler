using ll.type;
using System;

namespace ll.AST
{
    public class AndExpr : BinOp
    {
        public AndExpr(IAST left, IAST right): base(left, right, "&&", new BooleanType())
        {
            if(!(left.type is BooleanType) || !(right.type is BooleanType))
                throw new ArgumentException("And operator only accepts bool values as operands");
        }
    }
}