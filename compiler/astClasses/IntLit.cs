using ll.type;

namespace ll.AST
{
    public class IntLit : IAST
    {
        public long? n { get; set; }

        public IntLit(long? n) : base(new IntType())
        {
            this.n = n;
        }

        public override string ToString()
        {
            return n.ToString();
        }
    }
}