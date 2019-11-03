using System;

namespace ll.AST
{
    public class BinOp
    {
        public IAST left { get; set; }
        public IAST right { get; set; }
        public string op { get; set; }

        public BinOp(IAST left, IAST right, string op)
        {
            if(left is AssignExpr || right is AssignExpr)
                throw new ArgumentException("no assignExpression allowed in a binary operation");
            this.left = left;
            this.right = right;
            this.op = op;
        }
    }
}