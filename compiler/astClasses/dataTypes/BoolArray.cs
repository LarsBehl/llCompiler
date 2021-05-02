using LL.Types;
using System;

namespace LL.AST
{
    public class BoolArray : Array
    {

        public BoolArray(IAST capacity, IAST[] values, int line, int column) : base(capacity, values, new BoolArrayType(), line, column)
        {
        }

        public BoolArray(IAST capacity, int line, int column) : this(capacity, new IAST[0], line, column)
        {

        }

        public BoolArray(int line, int column) : this(new IntLit(-1, line, column), new IAST[0], line, column)
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
                    result += "null, ";
            }

            result = result.Substring(0, result.Length - 2) + "]";

            return result;
        }
    }
}