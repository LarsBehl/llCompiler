namespace ll.AST
{
    public class AddExpr : BinOp
    {
        public AddExpr(IAST left, IAST right) : base(left, right, "+")
        {

        }
    }
}