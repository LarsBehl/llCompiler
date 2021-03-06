using LL.Types;
using System;

namespace LL.AST
{
    public class OrExpr : BinOp
    {
        public OrExpr(IAST left, IAST right, int line, int column) : base(left, right, "||", new BooleanType(), line, column)
        {
            if (!(left.Type is BooleanType) || !(right.Type is BooleanType))
                throw new ArgumentException($"Or operator only accepts bool values as operands; On line {line}:{column}");
        }
    }
}