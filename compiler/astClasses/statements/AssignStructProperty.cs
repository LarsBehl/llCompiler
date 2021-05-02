using LL.Types;
using System;

namespace LL.AST
{
    public class AssignStructProperty : IAST
    {
        public StructPropertyAccess StructProp { get; set; }
        public IAST Value { get; set; }

        public AssignStructProperty(StructPropertyAccess structProp, IAST val, int line, int column) : base(new AssignStructPropertyType(), line, column)
        {
            if (structProp.Type != val.Type
                && !(structProp.Type is DoubleType && val.Type is IntType))
                throw new ArgumentException($"Type \"{structProp.Type.TypeName}\" is not compatible with \"{val.Type.TypeName}\"; On line {line}:{column}");

            this.StructProp = structProp;
            this.Value = val;
        }
    }
}