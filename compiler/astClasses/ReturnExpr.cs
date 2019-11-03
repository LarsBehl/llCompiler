using System;

namespace ll.AST
{
    public class ReturnExpr : IAST
    {
        public IAST returnValue { get; set; }

        public ReturnExpr(IAST returnValue)
        {
            if(returnValue is AssignExpr)
                throw new ArgumentException("returning assignExpression is not allowed");
            this.returnValue = returnValue;
        }
    }
}