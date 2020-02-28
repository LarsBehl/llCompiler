using ll.type;
using System;

namespace ll.AST
{
    public class NotEqualExpr : BinOp
    {
        public NotEqualExpr(IAST left, IAST right, int line, int column) : base(left, right, "!=", new BooleanType(), line, column)
        {
            this.CheckType();
        }

        private void CheckType()
        {
            bool hasToThrow = false;

            switch (this.left.type)
            {
                case DoubleType doubleType:
                    if (!(right.type is DoubleType || right.type is IntType))
                        hasToThrow = true;
                    break;
                case IntType intType:
                    if (!(right.type is DoubleType || right.type is IntType))
                        hasToThrow = true;
                    break;
                case BooleanType booleanType:
                    if (!(right.type is BooleanType))
                        hasToThrow = true;
                    break;
                default:
                    throw new ArgumentException($"Unknown type {this.left.type.typeName}; On line {this.line}:{this.column}");
            }

            if (hasToThrow)
                throw new ArgumentException($"Could not compare {this.left.type.typeName} with {this.right.type.typeName}; On line {this.line}:{this.column}");
        }
    }
}