using System;

namespace ll.AST
{
    public class ReturnStatement : IAST
    {
        public IAST returnValue { get; set; }

        public ReturnStatement(IAST returnValue) : base(returnValue.type)
        {
            this.returnValue = returnValue;
        }
    }
}