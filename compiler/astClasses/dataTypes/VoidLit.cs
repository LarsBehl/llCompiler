using ll.type;

namespace ll.AST
{
    public class VoidLit : IAST
    {
        public VoidLit(): base(new VoidType())
        {

        }
    }
}