using ll.type;
using System;

namespace ll.AST
{
    public class DoubleArray : Array
    {

        public DoubleArray(IAST capacity, IAST[] values) : base(capacity, values, new IntArrayType())
        {
            if (!(capacity.type is IntType))
                throw new ArgumentException("Size of an array has to be an integer");
        }

        public DoubleArray(IAST capacity) : this(capacity, new IAST[0])
        {

        }

        public DoubleArray() : this(new IntLit(-1), new IAST[0])
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