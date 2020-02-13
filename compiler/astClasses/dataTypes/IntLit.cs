using ll.type;

namespace ll.AST
{
    public class IntLit : IAST
    {
        public long? n { get; set; }

        public IntLit(long? n, int line, int column) : base(new IntType(), line, column)
        {
            this.n = n;
        }

        public override string ToString()
        {
            return n.ToString();
        }
    }
}