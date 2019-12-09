namespace ll.AST
{
    public class IncrementExpr : IAST
    {
        public VarExpr variable { get; set; }
        public bool post { get; set; }

        public IncrementExpr(VarExpr variable, bool post): base(variable.type)
        {
            this.variable = variable;
            this.post = post;
        }
    }
}