using ll.type;
using System;

namespace ll.AST
{
    public class StructPropertyAccess : IAST
    {
        public VarExpr structRef { get; set; }
        public string propName { get; set; }

        public StructPropertyAccess(VarExpr structRef, string propName, int line, int column) : base(GetType(structRef, propName), line, column)
        {
            this.structRef = structRef;
            this.propName = propName;
        }

        static ll.type.Type GetType(VarExpr structRef, string propName)
        {
            if (!(structRef.type is StructType structType))
                throw new ArgumentException($"Given variable does not reference a struct; references: {structRef.type.typeName}");

            if (!IAST.structs.ContainsKey(structType.structName))
                throw new ArgumentException($"Unknown struct {structType.structName}");

            StructDefinition structDefinition = IAST.structs[structType.structName];

            int propIndex = structDefinition.properties.FindIndex(p => p.name == propName);

            if (propIndex < 0)
                throw new ArgumentException($"Struct \"{structType.structName}\" has no property \"{propName}\"");

            return structDefinition.properties[propIndex].type;
        }
    }
}