namespace ll.AST
{
    public class VarExpr : IAST
    {
        public string name { get; set; }

        public VarExpr(string name)
        {
            this.name = name;
        }
    }
}