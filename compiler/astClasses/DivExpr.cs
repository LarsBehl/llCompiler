namespace ll.AST
{
    public class DivExpr : BinOp, IAST
    {
        public DivExpr(IAST left, IAST right) : base(left, right, "/")
        {

        }
    }
}