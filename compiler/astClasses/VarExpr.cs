using ll.AST;

namespace ll.AST
{
    public class VarExpr : IAST
    {
        public string name { get; set; }

        public VarExpr(string name) : base(IAST.env[name].type)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return this.name;
        }
    }
}