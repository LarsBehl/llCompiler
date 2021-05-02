using LL.Types;

namespace LL.AST
{
    public class DoubleLit : IAST
    {
        public double? Value { get; set; }

        public DoubleLit(double? n, int line, int column) : base(new DoubleType(), line, column)
        {
            this.Value = n;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}