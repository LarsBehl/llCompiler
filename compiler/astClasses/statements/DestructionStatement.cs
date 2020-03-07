using ll.type;
using System;

namespace ll.AST
{
    public class DestructionStatement : IAST
    {
        public ValueAccessExpression refType { get; set; }

        public DestructionStatement(ValueAccessExpression refType, int line, int column) : base(new DestructionStatementType(), line, column)
        {
            if (!(refType.type is RefType))
                throw new ArgumentException($"Coul dnot free non ref type \"{refType.type.typeName}\"; On line {line}:{column}");
            this.refType = refType;
        }
    }
}