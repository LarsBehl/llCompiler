using LL.Types;

namespace LL.AST
{
    public class VoidLit : IAST
    {
        public VoidLit(int line, int column) : base(new VoidType(), line, column)
        {

        }
    }
}