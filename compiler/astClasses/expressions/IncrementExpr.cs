namespace LL.AST
{
    public class IncrementExpr : IAST
    {
        public ValueAccessExpression Variable { get; set; }
        public bool Post { get; set; }

        public IncrementExpr(ValueAccessExpression variable, bool post, int line, int column) : base(variable.Type, line, column)
        {
            this.Variable = variable;
            this.Post = post;
        }
    }
}