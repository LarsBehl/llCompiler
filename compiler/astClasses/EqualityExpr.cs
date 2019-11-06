using System;
using ll.type;

namespace ll.AST
{
    public class EqualityExpr : BinOp
    {
        public EqualityExpr(IAST left, IAST right) : base(left, right, "==", GetType(left, right))
        {
            
        }

        static ll.type.Type GetType(IAST left, IAST right)
        {
            switch(left.type)
            {
                case IntType i:
                    if(right.type is IntType || right.type is DoubleType)
                        return new BooleanType();
                    throw new ArgumentException($"Can not compare {left.type} to {right.type}");
                case DoubleType d:
                    if(right.type is IntType || right.type is DoubleType)
                        return new BooleanType();
                    throw new ArgumentException($"Can not compare {left.type} to {right.type}");
                case BooleanType b:
                    if(!(right.type is BooleanType))
                        throw new ArgumentException($"Type {right.type} is not allowed for \"==\" operation");
                    return new BooleanType();
                default:
                    throw new ArgumentException($"Can not compare {left.type} to {right.type}");
            }
        }
    }
}