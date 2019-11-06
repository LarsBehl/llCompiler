namespace ll.AST
{
    public class LessExpr : BinOp
    {
        public LessExpr(IAST left, IAST right) : base(left, right, "<")
        {
            
        }
    }
}