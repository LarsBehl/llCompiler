namespace ll.AST
{
    public class GreaterExpr : BinOp
    {
        public GreaterExpr(IAST left, IAST right) : base(left, right, ">")
        {

        }
    }
}