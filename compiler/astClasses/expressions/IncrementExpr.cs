namespace ll.AST
{
    public class IncrementExpr : IAST
    {
        public ValueAccessExpression variable { get; set; }
        public bool post { get; set; }

        public IncrementExpr(ValueAccessExpression variable, bool post, int line, int column) : base(variable.type, line, column)
        {
            this.variable = variable;
            this.post = post;
        }
    }
}