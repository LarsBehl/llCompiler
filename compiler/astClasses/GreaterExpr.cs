using System;
using ll.type;

namespace ll.AST
{
    public class GreaterExpr : BinOp
    {
        public GreaterExpr(IAST left, IAST right) : base(left, right, ">", GetType(left, right))
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
                default:
                    throw new ArgumentException($"Can not compare {left.type} to {right.type}");
            }
        }
    }
}