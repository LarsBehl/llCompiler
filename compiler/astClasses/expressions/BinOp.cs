using System;
using ll.type;

namespace ll.AST
{
    public abstract class BinOp : IAST
    {
        public IAST left { get; set; }
        public IAST right { get; set; }
        public string op { get; set; }

        public BinOp(IAST left, IAST right, string op, ll.type.Type type, int line, int column) : base(type, line, column)
        {
            if (left is AssignStatement || right is AssignStatement)
                throw new ArgumentException($"no assignExpression allowed in a binary operation; On line {line}:{column}");
            this.left = left;
            this.right = right;
            this.op = op;
        }

    }
}