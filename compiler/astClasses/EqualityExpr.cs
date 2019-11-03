namespace ll.AST
{
    public class EqualityExpr : BinOp, IAST
    {
        public EqualityExpr(IAST left, IAST right) : base(left, right, "==")
        {
            
        }
    }
}