using System;
using LL.Types;

namespace LL.AST
{
    public class GreaterExpr : BinOp
    {
        public bool Equal { get; set; }

        public GreaterExpr(IAST left, IAST right, bool equal, int line, int column) : base(left, right, ">", GetType(left, right, line, column), line, column)
        {
            this.Equal = equal;
        }

        static LL.Types.Type GetType(IAST left, IAST right, int line, int column)
        {
            switch (left.Type)
            {
                case IntType i:
                    if (right.Type is IntType || right.Type is DoubleType)
                        return new BooleanType();
                    throw new ArgumentException($"Can not compare {left.Type} to {right.Type}; On line {line}:{column}");
                case DoubleType d:
                    if (right.Type is IntType || right.Type is DoubleType)
                        return new BooleanType();
                    throw new ArgumentException($"Can not compare {left.Type} to {right.Type}; On line {line}:{column}");
                default:
                    throw new ArgumentException($"Can not compare {left.Type} to {right.Type}; On line {line}:{column}");
            }
        }
    }
}