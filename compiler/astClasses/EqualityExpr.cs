namespace ll.AST
{
    public class EqualityExpr : BinOp
    {
        public EqualityExpr(IAST left, IAST right) : base(left, right, "==")
        {
            
        }
    }
}