namespace ll.AST
{
    public class VarExpr : IAST
    {
        public string name { get; set; }

        public VarExpr(string name) : base(null)
        {
            this.name = name;
        }
    }
}