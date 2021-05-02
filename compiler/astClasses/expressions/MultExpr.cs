using System;
using LL.Types;

namespace LL.AST
{
    public class MultExpr : BinOp
    {
        public MultExpr(IAST left, IAST right, int line, int column) : base(left, right, "*", GetType(left, right, line, column), line, column)
        {

        }

        static LL.Types.Type GetType(IAST left, IAST right, int line, int column)
        {
            switch (left.Type)
            {
                case IntType i:
                    if (right.Type is IntType)
                        return new IntType();
                    if (right.Type is DoubleType)
                        return new DoubleType();
                    throw new ArgumentException($"Type {right.Type} is not allowed for \"*\" operation; On line {line}:{column}");
                case DoubleType d:
                    if (right.Type is IntType || right.Type is DoubleType)
                        return new DoubleType();
                    throw new ArgumentException($"Type {right.Type} is not allowed for \"*\" operation; On line {line}:{column}");
                default:
                    throw new ArgumentException($"Type {left.Type} is not allowed for \"*\" operation; On line {line}:{column}");
            }
        }
    }
}