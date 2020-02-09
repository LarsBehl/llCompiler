using ll.type;

namespace ll.AST
{
    public class NullLit : IAST
    {
        public NullLit(int line, int column) : base(new NullType(), line, column)
        {

        }

        public override string ToString() => "null";
    }
}