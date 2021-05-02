namespace LL.AST
{
    public class RefTypeCreationStatement : IAST
    {
        public IAST CreatedReftype { get; set; }

        public RefTypeCreationStatement(IAST createdReftype, int line, int column) : base(createdReftype.Type, line, column)
        {
            this.CreatedReftype = createdReftype;
        }
    }
}