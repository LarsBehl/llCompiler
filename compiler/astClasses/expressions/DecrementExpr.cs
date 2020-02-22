namespace ll.AST
{
    public class DecrementExpr : IAST
    {
        public ValueAccessExpression variable { get; set; }
        public bool post { get; set; }

        public DecrementExpr(ValueAccessExpression variable, bool post, int line, int column) : base(variable.type, line, column)
        {
            this.variable = variable;
            this.post = post;
        }
    }
}