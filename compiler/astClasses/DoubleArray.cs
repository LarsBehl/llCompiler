using ll.type;
using System;

namespace ll.AST
{
    public class DoubleArray : IAST
    {
        public int capacity { get; set; }
        public IAST[] values { get; set; }

        public DoubleArray(int capacity, IAST[] values) : base(new IntArrayType())
        {
            this.capacity = capacity;

            foreach (IAST node in values)
            {
                if (!(node?.type is DoubleType))
                    throw new ArgumentException($"Could not save \"{node.type.typeName}\" values in DoubleArray");
            }

            this.values = values;
        }

        public DoubleArray(int capacity) : this(capacity, new IAST[capacity])
        {

        }

        public DoubleArray() : this(-1, new IAST[0])
        {

        }
    }
}