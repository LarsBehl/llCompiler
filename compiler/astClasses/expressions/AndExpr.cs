using ll.type;
using System;

namespace ll.AST
{
    public class AndExpr : BinOp
    {
        public AndExpr(IAST left, IAST right, int line, int column) : base(left, right, "&&", new BooleanType(), line, column)
        {
            if (!(left.type is BooleanType) || !(right.type is BooleanType))
                throw new ArgumentException($"And operator only accepts bool values as operands; On line {line}:{column}");
        }
    }
}