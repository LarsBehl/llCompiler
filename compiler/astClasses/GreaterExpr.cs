namespace ll
{
    public class GreaterExpr : BinOp, IAST
    {
        public GreaterExpr(IAST left, IAST right) : base(left, right, ">")
        {

        }
    }
}