namespace ll.AST
{
    public class MultExpr : BinOp, IAST
    {
        public MultExpr(IAST left, IAST right) : base(left, right, "*")
        {

        }
    }
}