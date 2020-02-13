using ll.type;

namespace ll.AST
{
    public class ReturnStatement : IAST
    {
        public IAST returnValue { get; set; }

        public ReturnStatement(int line, int column) : base(new VoidType(), line, column)
        {

        }

        public ReturnStatement(IAST returnValue, int line, int column) : base(returnValue.type, line, column)
        {
            this.returnValue = returnValue;
        }
    }
}