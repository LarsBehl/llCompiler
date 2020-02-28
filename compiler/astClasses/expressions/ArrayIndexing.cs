using System;
using ll.type;

namespace ll.AST
{
    public class ArrayIndexing : ValueAccessExpression
    {
        public IAST left { get; set; }
        public IAST right { get; set; }
        public ArrayIndexing(IAST array, IAST index, int line, int column) : base(GetType(array, line, column), line, column)
        {
            if (!(index.type is IntType))
                throw new ArgumentException($"The index of an array has to be an int; received: {index.type.typeName}; On line {line}:{column}");

            this.left = array;
            this.right = index;
        }

        static ll.type.PrimitivType GetType(IAST array, int line, int column)
        {
            switch (array.type)
            {
                case IntArrayType iat:
                    return new IntType();
                case DoubleArrayType dat:
                    return new DoubleType();
                case BoolArrayType bat:
                    return new BooleanType();
                default:
                    throw new ArgumentException($"Given object is not an array: {array.type.typeName}; On line {line}:{column}");
            }
        }
    }
}