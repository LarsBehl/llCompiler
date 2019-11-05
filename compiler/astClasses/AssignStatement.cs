namespace ll.AST
{
    public class AssignStatement : IAST 
    {
        public VarExpr variable { get; set; }
        public IAST value { get; set; }

        public AssignStatement(VarExpr variable, IAST value)
        {
            this.variable = variable;
            this.value = value;
        }
    }
}