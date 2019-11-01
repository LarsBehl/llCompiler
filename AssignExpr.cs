namespace ll
{
    public class AssignExpr : IAST
    {
        public VarExpr v { get; set; }
        public IAST val { get; set; }

        public AssignExpr(VarExpr var, IAST val)
        {
            this.v = var;
            this.val = val;
        }
    }
}