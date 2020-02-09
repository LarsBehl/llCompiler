using ll.type;

namespace ll.AST
{
    public class PrintStatement : IAST
    {
        public IAST value { get; set; }
        public PrintStatement(IAST value, int line, int column) : base(new PrintStatementType(), line, column)
        {
            this.value = value;
        }
    }
}