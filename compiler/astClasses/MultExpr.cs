using System;
using ll.type;

namespace ll.AST
{
    public class MultExpr : BinOp
    {
        public MultExpr(IAST left, IAST right) : base(left, right, "*", GetType(left, right))
        {

        }

        static ll.type.Type GetType(IAST left, IAST right)
        {
            switch(left.type)
            {
                case IntType i:
                    if(right.type is IntType)
                        return new IntType();
                    if(right.type is DoubleType)
                        return new DoubleType();
                    throw new ArgumentException($"Type {right.type} is not allowed for \"*\" operation");
                case DoubleType d:
                    if(right.type is IntType || right.type is DoubleType)
                        return new DoubleType();
                    throw new ArgumentException($"Type {right.type} is not allowed for \"*\" operation");
                default:
                    throw new ArgumentException($"Type {left.type} is not allowed for \"*\" operation");
            }
        }
    }
}