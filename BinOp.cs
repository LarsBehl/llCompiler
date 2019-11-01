using System;

namespace ll
{
    public class BinOp
    {
        public IAST left { get; set; }
        public IAST right { get; set; }
        public string op { get; set; }

        public BinOp(IAST left, IAST right, string op)
        {
            this.left = left;
            this.right = right;
            this.op = op;
        }
    }
}