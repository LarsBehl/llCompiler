namespace ll
{
    public class SubExpr : BinOp, IAST
    {
        public SubExpr(IAST left, IAST right) : base(left, right, "-")
        {

        }
    }
}