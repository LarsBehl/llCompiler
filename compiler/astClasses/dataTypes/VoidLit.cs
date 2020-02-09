using ll.type;

namespace ll.AST
{
    public class VoidLit : IAST
    {
        public VoidLit(int line, int column) : base(new VoidType(), line, column)
        {

        }
    }
}