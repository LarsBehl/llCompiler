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
            if((structProp.type is StructType propType) && (val.type is StructType valType))
            {
                if(propType.structName != valType.structName)
                    throw new ArgumentException($"Type \"{structProp.type.typeName}\" is not compatible with \"{val.type.typeName}\"; On line {line}:{column}");
            }

            if (structProp.type.typeName != val.type.typeName && !(structProp.type is RefType && val.type is NullType))
                throw new ArgumentException($"Type \"{structProp.type.typeName}\" is not compatible with \"{val.type.typeName}\"; On line {line}:{column}");

            this.structProp = structProp;
            this.val = val;
        }
    }
}