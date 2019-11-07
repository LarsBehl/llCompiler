using ll.type;

namespace ll.AST
{
    public class BoolLit : IAST
    {
        public bool value { get; set; }

        public BoolLit(bool value) : base(new BooleanType())
        {
            this.value = value;
        }
    }
}