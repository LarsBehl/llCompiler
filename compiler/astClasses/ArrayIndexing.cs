using System;
using ll.type;

namespace ll.AST
{
    public class ArrayIndexing : BinOp
    {
        public ArrayIndexing(IAST array, IAST index) : base(array, index, "[]", GetType(array))
        {
            if (!(index.type is IntType))
                throw new ArgumentException($"The index of an array has to be an int; received: {index.type.typeName}");
        }

        static ll.type.PrimitivType GetType(IAST array)
        {
            switch (array.type)
            {
                case IntArrayType iat:
                    return new IntType();
                case DoubleArrayType dat:
                    return new IntType();
                case BoolArrayType bat:
                    return new BooleanType();
                default:
                    throw new ArgumentException($"Given object is not an array: {array.type.typeName}");
            }
        }
    }
}