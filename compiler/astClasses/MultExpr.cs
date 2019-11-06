namespace ll.AST
{
    public class MultExpr : BinOp
    {
        public MultExpr(IAST left, IAST right) : base(left, right, "*")
        {

        }
    }
}