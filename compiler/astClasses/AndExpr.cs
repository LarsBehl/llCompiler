using ll.type;

namespace ll.AST
{
    public class AndExpr : BinOp
    {
        public AndExpr(IAST left, IAST right): base(left, right, "&&", new BooleanType())
        {

        }
    }
}