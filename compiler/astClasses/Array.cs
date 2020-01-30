using ll.type;

namespace ll.AST
{
    public class Array : IAST
    {
        public IAST size { get; set; }
        public IAST[] values { get; set; }

        public Array(IAST size, IAST[] values, type.Type type) : base(type)
        {
            this.size = size;
            this.values = values;
        }
    }
}