namespace LL.AST
{
    public class DecrementExpr : IAST
    {
        public ValueAccessExpression Variable { get; set; }
        public bool Post { get; set; }

        public DecrementExpr(ValueAccessExpression variable, bool post, int line, int column) : base(variable.Type, line, column)
        {
            this.Variable = variable;
            this.Post = post;
        }
    }
}