using LL.Types;

namespace LL.AST
{
    public class StringLit : IAST
    {
        public string Value { get; set; }
        /// <value>
        /// Number of characters in current string, not including terminating null
        /// </value>
        public int Length { get; set; }

        public StringLit(string value, int line, int column) : base(new CharArrayType(), line, column)
        {
            this.Value = value;
            this.Length = value.Length;
        }

        public override string ToString() => $"\"{Value}\"";
    }
}