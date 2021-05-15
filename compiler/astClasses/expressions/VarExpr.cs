using LL.Types;
using LL.Exceptions;

namespace LL.AST
{
    public class VarExpr : ValueAccessExpression
    {
        public string Name { get; set; }

        public VarExpr(string name, LL.Types.Type type, int line, int column): base(type, line, column)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return this.Name;
        }
    }
}