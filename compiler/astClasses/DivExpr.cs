namespace ll.AST
{
    public class DivExpr : BinOp
    {
        public DivExpr(IAST left, IAST right) : base(left, right, "/")
        {

        }
    }
}