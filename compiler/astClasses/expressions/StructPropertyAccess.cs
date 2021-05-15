using LL.Types;
using System;

namespace LL.AST
{
    public class StructPropertyAccess : ValueAccessExpression
    {
        public VarExpr StructRef { get; set; }
        public ValueAccessExpression Prop { get; set; }

        public StructPropertyAccess(VarExpr structRef, ValueAccessExpression prop, int line, int column) : base(prop.Type, line, column)
        {
            this.StructRef = structRef;
            this.Prop = prop;
        }
    }
}