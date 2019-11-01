namespace ll
{
    public class AddExpr : BinOp, IAST
    {
        public AddExpr(IAST left, IAST right) : base(left, right, "+")
        {

        }
    }
}