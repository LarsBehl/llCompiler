using LL.Types;
using System;

namespace LL.AST
{
    public class AndExpr : BinOp
    {
        public AndExpr(IAST left, IAST right, int line, int column) : base(left, right, "&&", new BooleanType(), line, column)
        {
            if (!(left.Type is BooleanType) || !(right.Type is BooleanType))
                throw new ArgumentException($"And operator only accepts bool values as operands; On line {line}:{column}");
        }
    }
}