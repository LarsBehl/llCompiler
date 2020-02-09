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
            if (structProp.type.typeName != val.type.typeName)
                throw new ArgumentException($"Type \"{structProp.type.typeName}\" is not compatible with \"{val.type.typeName}\"");

            this.structProp = structProp;
            this.val = val;
        }
    }
}