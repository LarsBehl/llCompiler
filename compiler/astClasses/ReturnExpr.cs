namespace ll
{
    public class ReturnExpr : IAST
    {
        public IAST returnValue { get; set; }

        public ReturnExpr(IAST returnValue)
        {
            this.returnValue = returnValue;
        }
    }
}