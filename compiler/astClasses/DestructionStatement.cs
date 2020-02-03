using ll.type;

namespace ll.AST
{
    public class DestructionStatement : IAST
    {
        public VarExpr refType { get; set; }

        public DestructionStatement(VarExpr refType) : base(new DestructionStatementType())
        {
            this.refType = refType;
        }
    }
}