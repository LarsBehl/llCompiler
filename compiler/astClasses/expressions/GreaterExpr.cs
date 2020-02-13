using System;
using ll.type;

namespace ll.AST
{
    public class GreaterExpr : BinOp
    {
        public bool equal { get; set; }
        public GreaterExpr(IAST left, IAST right, bool equal, int line, int column) : base(left, right, ">", GetType(left, right, line, column), line, column)
        {
            this.equal = equal;
        }

        static ll.type.Type GetType(IAST left, IAST right, int line, int column)
        {
            switch (left.type)
            {
                case IntType i:
                    if (right.type is IntType || right.type is DoubleType)
                        return new BooleanType();
                    throw new ArgumentException($"Can not compare {left.type} to {right.type}; On line {line}:{column}");
                case DoubleType d:
                    if (right.type is IntType || right.type is DoubleType)
                        return new BooleanType();
                    throw new ArgumentException($"Can not compare {left.type} to {right.type}; On line {line}:{column}");
                default:
                    throw new ArgumentException($"Can not compare {left.type} to {right.type}; On line {line}:{column}");
            }
        }
    }
}