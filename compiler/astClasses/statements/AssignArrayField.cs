using System;
using LL.Types;

namespace LL.AST
{
    public class AssignArrayField : IAST
    {
        public ArrayIndexing ArrayIndex { get; set; }
        public IAST Value { get; set; }

        public AssignArrayField(ArrayIndexing arrayIndexing, IAST value, int line, int column) : base(arrayIndexing.Type, line, column)
        {
            if (arrayIndexing.Type != value.Type)
            {
                if(arrayIndexing.Type is not DoubleType || value.Type is not IntType)
                    throw new ArgumentException($"Could not assign \"{value.Type.typeName}\" to an \"{arrayIndexing.Type.typeName}\" array; On line {line}:{column}");
            }

            this.ArrayIndex = arrayIndexing;
            this.Value = value;
        }
    }
}