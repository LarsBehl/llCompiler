using LL.Types;

namespace LL.AST
{
    public class BoolLit : IAST
    {
        public bool? Value { get; set; }

        public BoolLit(bool? value, int line, int column) : base(new BooleanType(), line, column)
        {
            this.Value = value;
        }

        public override string ToString()
        {
            return Value.ToString();
        }
    }
}