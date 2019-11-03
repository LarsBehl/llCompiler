namespace ll.AST
{
    public class AssignExpr : IAST 
    {
        public VarExpr variable { get; set; }
        public IAST value { get; set; }

        public AssignExpr(VarExpr variable, IAST value)
        {
            this.variable = variable;
            this.value = value;
        }
    }
}