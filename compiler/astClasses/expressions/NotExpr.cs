using LL.Types;
using System;

namespace LL.AST
{
    public class NotExpr : IAST
    {
        public IAST Value { get; set; }

        public NotExpr(IAST value, int line, int column) : base(new BooleanType(), line, column)
        {
            if (!(value.Type is BooleanType))
                throw new ArgumentException($"\"NotOperator\" only viable for BoolType; On line {line}:{column}");

            this.Value = value;
        }
    }
}