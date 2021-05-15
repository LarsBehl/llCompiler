using LL.Types;

namespace LL.AST
{
    public class ReturnStatement : IAST
    {
        public IAST ReturnValue { get; set; }

        public ReturnStatement(int line, int column) : base(new VoidType(), line, column)
        {

        }

        public ReturnStatement(IAST returnValue, int line, int column) : base(returnValue.Type, line, column)
        {
            this.ReturnValue = returnValue;
        }
    }
}