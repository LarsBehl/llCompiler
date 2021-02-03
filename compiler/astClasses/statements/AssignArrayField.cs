using System;
using ll.type;

namespace ll.AST
{
    public class AssignArrayField : IAST
    {
        public ArrayIndexing arrayIndex { get; set; }
        public IAST value { get; set; }

        public AssignArrayField(ArrayIndexing arrayIndexing, IAST value, int line, int column) : base(arrayIndexing.type, line, column)
        {
            if (arrayIndexing.type.typeName != value.type.typeName)
            {
                if(arrayIndexing.type is not DoubleType || value.type is not IntType)
                    throw new ArgumentException($"Could not assign \"{value.type.typeName}\" to an \"{arrayIndexing.type.typeName}\" array; On line {line}:{column}");
            }

            this.arrayIndex = arrayIndexing;
            this.value = value;
        }
    }
}