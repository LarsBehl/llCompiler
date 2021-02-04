using ll.type;
using System;

namespace ll.AST
{
    public class AssignStructProperty : IAST
    {
        public StructPropertyAccess structProp { get; set; }
        public IAST val { get; set; }

        public AssignStructProperty(StructPropertyAccess structProp, IAST val, int line, int column) : base(new AssignStructPropertyType(), line, column)
        {
            if (structProp.type != val.type
                && !(structProp.type is DoubleType && val.type is IntType))
                throw new ArgumentException($"Type \"{structProp.type.typeName}\" is not compatible with \"{val.type.typeName}\"; On line {line}:{column}");

            this.structProp = structProp;
            this.val = val;
        }
    }
}