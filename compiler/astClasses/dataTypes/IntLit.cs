using LL.Types;

namespace LL.AST
{
    public class IntLit : IAST
    {
        public long? Value { get; set; }

        public IntLit(long? n, int line, int column) : base(new IntType(), line, column)
        {
            this.Value = n;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}