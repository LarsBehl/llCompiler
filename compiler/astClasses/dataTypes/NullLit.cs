using ll.type;

namespace ll.AST
{
    public class NullLit : IAST
    {
        public NullLit() : base(new NullType())
        {

        }

        public override string ToString() => "null";
    }
}