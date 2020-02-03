using ll.type;
using System;

namespace ll.AST
{
    public class BoolArray : Array
    {

        public BoolArray(IAST capacity, IAST[] values) : base(capacity, values, new BoolArrayType())
        {
        }

        public BoolArray(IAST capacity) : this(capacity, new IAST[0])
        {

        }

        public BoolArray() : this(new IntLit(-1), new IAST[0])
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