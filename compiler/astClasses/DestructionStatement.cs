using ll.type;
using System;

namespace ll.AST
{
    public class DestructionStatement : IAST
    {
        public VarExpr refType { get; set; }

        public DestructionStatement(VarExpr refType) : base(new DestructionStatementType())
        {
            if (!(refType.type is RefType))
                throw new ArgumentException($"Coul dnot free non ref type \"{refType.type.typeName}\"");
            this.refType = refType;
        }
    }
}