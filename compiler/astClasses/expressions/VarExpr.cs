using ll.AST;

namespace ll.AST
{
    public class VarExpr : IAST
    {
        public string name { get; set; }

    	// TODO maybe add property wether the variable is initialized or not
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