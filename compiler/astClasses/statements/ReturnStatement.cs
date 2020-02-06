using ll.type;

namespace ll.AST
{
    public class ReturnStatement : IAST
    {
        public IAST returnValue { get; set; }

        public ReturnStatement(): base(new VoidType())
        {

        }
        
        public ReturnStatement(IAST returnValue) : base(returnValue.type)
        {
            this.returnValue = returnValue;
        }
    }
}