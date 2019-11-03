namespace ll.AST
{
    public class LessExpr : BinOp, IAST
    {
        public LessExpr(IAST left, IAST right) : base(left, right, "<")
        {
            
        }
    }
}