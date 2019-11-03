namespace ll.AST
{
    public class SubExpr : BinOp, IAST
    {
        public SubExpr(IAST left, IAST right) : base(left, right, "-")
        {

        }
    }
}