using LL.Types;

namespace LL.AST
{
    public class CharArray : Array
    {
        public CharArray(IAST capacity, IAST[] values, int line, int column) : base(capacity, values, new CharArrayType(), line, column)
        {

        }

        public CharArray(IAST capacity, int line, int column) : this(capacity, new IAST[0], line, column)
        {

        }

        public CharArray(int line, int column) : this(new IntLit(-1, line, column), new IAST[0], line, column)
        {

        }

        public override string ToString()
        {
            string result = "[";

            foreach(IAST node in this.Values)
            {
                if(node != null)
                    result += node.Eval().ToString() + ", ";
                else
                    result += "null, ";
            }

            result = result.Substring(0, result.Length - 2) + "]";

            return result;
        }
    }
}