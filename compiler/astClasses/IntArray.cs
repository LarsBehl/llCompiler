using ll.type;
using System;

namespace ll.AST
{
    public class IntArray : IAST
    {
        public int capacity { get; set; }
        public IAST[] values { get; set; }

        public IntArray(int capacity, IAST[] values) : base(new IntArrayType())
        {
            this.capacity = capacity;

            foreach (IAST node in values)
            {
                if (!(node?.type is IntType))
                    throw new ArgumentException($"Could not save \"{node.type.typeName}\" values in IntArray");
            }

            this.values = values;
        }

        public IntArray(int capacity) : this(capacity, new IAST[capacity])
        {

        }

        public IntArray() : this(-1, new IAST[0])
        {

        }
    }
}