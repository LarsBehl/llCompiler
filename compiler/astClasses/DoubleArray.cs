using ll.type;
using System;

namespace ll.AST
{
    public class DoubleArray : IAST
    {
        public IAST capacity { get; set; }
        public IAST[] values { get; set; }

        public DoubleArray(IAST capacity, IAST[] values) : base(new IntArrayType())
        {
            if (!(capacity.type is IntType))
                throw new ArgumentException("Size of an array has to be an integer");
            this.capacity = capacity;

            this.values = values;
        }

        public DoubleArray(IAST capacity) : this(capacity, null)
        {

        }

        public DoubleArray() : this(new IntLit(-1), null)
        {

        }

        public override string ToString()
        {
            string result = "[";

            foreach (IAST node in this.values)
            {
                if (node != null)
                    result += node.Eval().ToString() + ", ";
                else
                    result += "null, ";
            }

            result = result.Substring(0, result.Length - 2) + "]";

            return result;
        }
    }
}