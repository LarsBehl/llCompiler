using ll.type;
using System;

namespace ll.AST
{
    public class Array : IAST
    {
        public IAST size { get; set; }
        public IAST[] values { get; set; }

        public Array(IAST size, IAST[] values, type.Type type) : base(type)
        {
            if (!(size.type is IntType))
                throw new ArgumentException($"The size of an Array has to be an int; received: {size.type.typeName}");
            this.size = size;
            this.values = values;
        }
    }
}