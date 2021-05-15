using LL.Types;

namespace LL.AST
{
    public class PrintStatement : IAST
    {
        public IAST Value { get; set; }

        public PrintStatement(IAST value, int line, int column) : base(new PrintStatementType(), line, column)
        {
            this.Value = value;
        }
    }
}