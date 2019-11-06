using System;

namespace ll.AST
{
    public abstract class BinOp : IAST
    {
        public IAST left { get; set; }
        public IAST right { get; set; }
        public string op { get; set; }

        public BinOp(IAST left, IAST right, string op)
        {
            if(left is AssignStatement || right is AssignStatement)
                throw new ArgumentException("no assignExpression allowed in a binary operation");
            this.left = left;
            this.right = right;
            this.op = op;
        }
    }
}