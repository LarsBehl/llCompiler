using ll.type;

namespace ll.AST
{
    public class RefTypeCreationStatement : IAST
    {
        public IAST createdReftype { get; set; }

        public RefTypeCreationStatement(IAST createdReftype, int line, int column) : base(createdReftype.type, line, column)
        {
            this.createdReftype = createdReftype;
        }
    }
}