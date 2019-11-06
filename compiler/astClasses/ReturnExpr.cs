using System;

namespace ll.AST
{
    public class ReturnExpr : IAST
    {
        public IAST returnValue { get; set; }

        public ReturnExpr(IAST returnValue) : base(returnValue.type)
        {
            this.returnValue = returnValue;
        }
    }
}