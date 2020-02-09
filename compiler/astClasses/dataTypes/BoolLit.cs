using ll.type;

namespace ll.AST
{
    public class BoolLit : IAST
    {
        public bool? value { get; set; }

        public BoolLit(bool? value, int line, int column) : base(new BooleanType(), line, column)
        {
            this.value = value;
        }

        public override string ToString()
        {
            return value.ToString();
        }
    }
}