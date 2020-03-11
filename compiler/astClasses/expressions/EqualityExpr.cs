using System;
using ll.type;

namespace ll.AST
{
    public class EqualityExpr : BinOp
    {
        public EqualityExpr(IAST left, IAST right, int line, int column) : base(left, right, "==", new BooleanType(), line, column)
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
                case ArrayType arrayType:
                    if (!(right.type is ArrayType) && !(right.type is NullType)
                        || (right.type is ArrayType at) && at.typeName != arrayType.typeName)
                        hasToThrow = true;
                    break;
                case StructType structType:
                    if (!(right.type is StructType) && !(right.type is NullType)
                        || (right.type is StructType st) && st.structName != structType.structName)
                        hasToThrow = true;
                    break;
                case NullType nullType:
                    if (!(right.type is StructType) && !(right.type is NullType) && !(right.type is ArrayType))
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