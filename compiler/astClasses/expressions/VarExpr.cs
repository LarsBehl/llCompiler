using LL.Types;
using LL.Exceptions;

namespace LL.AST
{
    public class VarExpr : ValueAccessExpression
    {
        public string Name { get; set; }

        public VarExpr(string name, int line, int column) : base(TryGetType(name, line, column), line, column)
        {
            this.Name = name;
        }

        public VarExpr(string name, LL.Types.Type type, int line, int column): base(type, line, column)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return this.Name;
        }

        private static Type TryGetType(string varName, int line, int column)
        {
            bool success = IAST.Env.TryGetValue(varName, out IAST @var);

            if(!success)
                throw new UnknownVariableException(varName, null, line, column);
            
            return @var.Type;
        }
    }
}