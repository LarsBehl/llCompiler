using ll.type;

namespace ll.AST
{
    public class PrintStatement : IAST
    {
        public IAST value { get; set; }
        public PrintStatement(IAST value): base(new PrintStatementType())
        {
            this.value = value;
        }
    }
}