using ll.type;

namespace ll.AST
{
    public class RefTypeCreationStatement : IAST
    {
        public IAST createdReftype { get; set; }

        public RefTypeCreationStatement(IAST createdReftype) : base(createdReftype.type)
        {
            this.createdReftype = createdReftype;
        }
    }
}