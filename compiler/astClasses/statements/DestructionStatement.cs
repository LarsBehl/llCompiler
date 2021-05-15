using LL.Types;
using System;

namespace LL.AST
{
    public class DestructionStatement : IAST
    {
        public ValueAccessExpression RefType { get; set; }

        public DestructionStatement(ValueAccessExpression refType, int line, int column) : base(new DestructionStatementType(), line, column)
        {
            if (!(refType.Type is RefType))
                throw new ArgumentException($"Coul dnot free non ref type \"{refType.Type.TypeName}\"; On line {line}:{column}");
            this.RefType = refType;
        }
    }
}