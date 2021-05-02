using LL.Types;
using System;

namespace LL.AST
{
    public class ModExpr : BinOp
    {
        public ModExpr(IAST left, IAST right, int line, int column) : base(left, right, "%", new IntType(), line, column)
        {
            if (!(left.Type is IntType))
                throw new ArgumentException($"Could not use \"{left.Type.typeName}\" with modulo; On line {this.Line}:{this.Column}");

            if (!(right.Type is IntType))
                throw new ArgumentException($"Could not use \"{left.Type.typeName}\" with modulo; On line {this.Line}:{this.Column}");
        }
    }
}