using System;
using ll.type;

namespace ll.AST
{
    public class EqualityExpr : BinOp
    {
        public EqualityExpr(IAST left, IAST right, int line, int column) : base(left, right, "==", GetType(left, right, line, column), line, column)
        {

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
                case BooleanType b:
                    if (!(right.type is BooleanType))
                        throw new ArgumentException($"Type {right.type} is not allowed for \"==\" operation; On line {line}:{column}");
                    return new BooleanType();
                default:
                    throw new ArgumentException($"Can not compare {left.type} to {right.type}; On line {line}:{column}");
            }
        }
    }
}