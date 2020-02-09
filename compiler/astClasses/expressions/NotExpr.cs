using ll.type;
using System;

namespace ll.AST
{
    public class NotExpr : IAST
    {
        public IAST value { get; set; }

        public NotExpr(IAST value, int line, int column) : base(new BooleanType(), line, column)
        {
            if (!(value.type is BooleanType))
                throw new ArgumentException("\"NotOperator\" only viable for BoolType");

            this.value = value;
        }
    }
}