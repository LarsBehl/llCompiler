using ll.type;

namespace ll.AST
{
    public class DoubleLit : IAST
    {
        public double? n { get; set; }

        public DoubleLit(double? n, int line, int column) : base(new DoubleType(), line, column)
        {
            this.n = n;
        }

        public override string ToString()
        {
            return n.ToString();
        }
    }
}