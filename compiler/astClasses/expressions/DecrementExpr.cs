namespace ll.AST
{
    public class DecrementExpr : IAST
    {
        public VarExpr variable { get; set; }
        public bool post { get; set; }

        public DecrementExpr(VarExpr variable, bool post): base(variable.type)
        {
            this.variable = variable;
            this.post = post;
        }
    }
}