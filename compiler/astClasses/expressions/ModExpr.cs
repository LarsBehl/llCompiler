using ll.type;
using System;

namespace ll.AST
{
    public class ModExpr : BinOp
    {
        public ModExpr(IAST left, IAST right, int line, int column) : base(left, right, "%", new IntType(), line, column)
        {
            if (!(left.type is IntType))
                throw new ArgumentException($"Could not use \"{left.type.typeName}\" with modulo; On line {this.line}:{this.column}");

            if (!(right.type is IntType))
                throw new ArgumentException($"Could not use \"{left.type.typeName}\" with modulo; On line {this.line}:{this.column}");
        }
    }
}