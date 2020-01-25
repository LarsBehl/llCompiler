using ll.type;
using System;

namespace ll.AST
{
    public class BoolArray : IAST
    {
        public int capacity { get; set; }
        public IAST[] values { get; set; }

        public BoolArray(int capacity, IAST[] values) : base(new BoolArrayType())
        {
            this.capacity = capacity;

            foreach (IAST node in values)
            {
                if (!(node?.type is BooleanType))
                    throw new ArgumentException($"Could not save \"{node.type.typeName}\" values in BoolArray");
            }

            this.values = values;
        }

        public BoolArray(int capacity) : this(capacity, new IAST[capacity])
        {

        }

        public BoolArray() : this(-1, new IAST[0])
        {

        }
    }
}