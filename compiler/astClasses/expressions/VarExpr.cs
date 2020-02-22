using ll.AST;

namespace ll.AST
{
    public class VarExpr : ValueAccessExpression
    {
        public string name { get; set; }

        public VarExpr(string name, int line, int column) : base(IAST.env[name].type, line, column)
        {
            this.name = name;
        }

        public override string ToString()
        {
            return this.name;
        }
    }
}