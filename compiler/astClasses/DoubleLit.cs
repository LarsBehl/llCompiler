using ll.type;

namespace ll.AST
{
    public class DoubleLit : IAST
    {
        public double? n { get; set; }

        public DoubleLit(double? n) : base(new DoubleType())
        {
            this.n = n;
        }

        public override string ToString()
        {
            return n.ToString();
        }
    }
}