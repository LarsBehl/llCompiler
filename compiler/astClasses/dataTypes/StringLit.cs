using LL.Types;

namespace LL.AST
{
    public class StringLit : IAST
    {
        public string Value { get; set; }

        public StringLit(string value, int line, int column) : base(new CharArrayType(), line, column)
        {
            this.Value = value;
        }

        public override string ToString() => $"\"{Value}\"";
    }
}