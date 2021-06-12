using LL.Types;
using System;

namespace LL.AST
{
    public class IntArray : Array
    {
        public IntArray(IAST capacity, IAST[] values, int line, int column) : base(capacity, values, new IntArrayType(), line, column)
        {
        }

        public IntArray(IAST capacity, int line, int column) : this(capacity, new IAST[0], line, column)
        {

        }

        public IntArray(int line, int column) : this(new IntLit(-1, line, column), new IAST[0], line, column)
        {

        }

        public override string ToString()
        {
            string result = "[";

            foreach (IAST node in this.Values)
            {
                if (node != null)
                    result += node.Eval().ToString() + ", ";
                else
                    result += "0, ";
            }

            result = result.Substring(0, result.Length - 2) + "]";

            return result;
        }
    }
}