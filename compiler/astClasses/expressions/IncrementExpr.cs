namespace ll.AST
{
    public class IncrementExpr : IAST
    {
        public VarExpr variable { get; set; }
        public bool post { get; set; }

        public IncrementExpr(VarExpr variable, bool post, int line, int column) : base(variable.type, line, column)
        {
            this.variable = variable;
            this.post = post;
        }
    }
}