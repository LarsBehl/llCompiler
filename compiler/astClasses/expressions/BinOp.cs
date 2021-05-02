using System;
using LL.Types;

namespace LL.AST
{
    public abstract class BinOp : IAST
    {
        public IAST Left { get; set; }
        public IAST Right { get; set; }
        public string Op { get; set; }

        public BinOp(IAST left, IAST right, string op, LL.Types.Type type, int line, int column) : base(type, line, column)
        {
            if (left is AssignStatement || right is AssignStatement)
                throw new ArgumentException($"no assignExpression allowed in a binary operation; On line {line}:{column}");
            this.Left = left;
            this.Right = right;
            this.Op = op;
        }

    }
}