using LL.Types;
using System;

namespace LL.AST
{
    public class Array : IAST
    {
        public IAST Size { get; set; }
        public IAST[] Values { get; set; }

        public Array(IAST size, IAST[] values, Types.Type type, int line, int column) : base(type, line, column)
        {
            if (!(size.Type is IntType))
                throw new ArgumentException($"The size of an Array has to be an int; received: {size.Type.typeName}; On line {line}:{column}");
            this.Size = size;
            this.Values = values;
        }
    }
}