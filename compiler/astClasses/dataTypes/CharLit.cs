using LL.Types;

namespace LL.AST
{
    public class CharLit : IAST
    {
        public char? Value { get; set; }

        public CharLit(int line, int column) : this(null, line, column)
        {
            
        }
        public CharLit(char? value, int line, int column) : base(new CharType(), line, column) => this.Value = value;

        public override string ToString() => Value.ToString();
    }
}