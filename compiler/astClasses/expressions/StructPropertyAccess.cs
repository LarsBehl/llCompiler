using ll.type;
using System;

namespace ll.AST
{
    public class StructPropertyAccess : ValueAccessExpression
    {
        public VarExpr structRef { get; set; }
        public ValueAccessExpression prop { get; set; }

        public StructPropertyAccess(VarExpr structRef, ValueAccessExpression prop, int line, int column) : base(prop.type, line, column)
        {
            this.structRef = structRef;
            this.prop = prop;
        }
    }
}