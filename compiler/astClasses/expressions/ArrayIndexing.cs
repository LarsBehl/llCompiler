using System;
using LL.Types;

namespace LL.AST
{
    public class ArrayIndexing : ValueAccessExpression
    {
        public IAST Left { get; set; }
        public IAST Right { get; set; }
        public ArrayIndexing(IAST array, IAST index, int line, int column) : base(GetType(array, line, column), line, column)
        {
            if (!(index.Type is IntType))
                throw new ArgumentException($"The index of an array has to be an int; received: {index.Type.TypeName}; On line {line}:{column}");

            this.Left = array;
            this.Right = index;
        }

        static LL.Types.PrimitivType GetType(IAST array, int line, int column)
        {
            switch (array.Type)
            {
                case IntArrayType iat:
                    return new IntType();
                case DoubleArrayType dat:
                    return new DoubleType();
                case BoolArrayType bat:
                    return new BooleanType();
                default:
                    throw new ArgumentException($"Given object is not an array: {array.Type.TypeName}; On line {line}:{column}");
            }
        }
    }
}