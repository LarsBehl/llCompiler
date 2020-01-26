using ll.type;
using System;

namespace ll.AST
{
    public class BoolArray : IAST
    {
        public IAST capacity { get; set; }
        public IAST[] values { get; set; }

        public BoolArray(IAST capacity, IAST[] values) : base(new BoolArrayType())
        {
            if (!(capacity.type is IntType))
                throw new ArgumentException("Size of an array has to be an integer");
            this.capacity = capacity;

            this.values = values;
        }

        public BoolArray(IAST capacity) : this(capacity, null)
        {

        }

        public BoolArray() : this(new IntLit(-1), null)
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