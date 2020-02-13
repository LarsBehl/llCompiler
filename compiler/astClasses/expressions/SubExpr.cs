using System;
using ll.type;

namespace ll.AST
{
    public class SubExpr : BinOp
    {
        public SubExpr(IAST left, IAST right, int line, int column) : base(left, right, "-", GetType(left, right, line, column), line, column)
        {

        }

        static ll.type.Type GetType(IAST left, IAST right, int line, int column)
        {
            switch (left.type)
            {
                case IntType i:
                    if (right.type is IntType)
                        return new IntType();
                    if (right.type is DoubleType)
                        return new DoubleType();
                    throw new ArgumentException($"Type {right.type} is not allowed for \"-\" operation; On line {line}:{column}");
                case DoubleType d:
                    if (right.type is IntType || right.type is DoubleType)
                        return new DoubleType();
                    throw new ArgumentException($"Type {right.type} is not allowed for \"-\" operation; On line {line}:{column}");
                default:
                    throw new ArgumentException($"Type {left.type} is not allowed for \"-\" operation; On line {line}:{column}");
            }
        }
    }
}