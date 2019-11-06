namespace ll.AST
{
    public class SubExpr : BinOp
    {
        public SubExpr(IAST left, IAST right) : base(left, right, "-")
        {

        }
    }
}