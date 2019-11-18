using ll.type;

namespace ll.AST
{
    public class IntLit : IAST
    {
        public int? n { get; set; }

        public IntLit(int? n) : base(new IntType())
        {
            this.n = n;
        }

        public override string ToString()
        {
            return n.ToString();
        }
    }
}