using System;
using ll.type;

namespace ll.AST
{
    public class MultAssignStatement : IAST
    {
        public VarExpr left { get; set; }
        public IAST right { get; set; }

        public MultAssignStatement(VarExpr left, IAST right): base(new MultAssignStatementType())
        {
            if(left.type.typeName != right.type.typeName)
                throw new ArgumentException($"Type of variable \"{left.type.typeName}\" does not match \"{right.type.typeName}\"");
            
            this.left = left;
            this.right = right;
        }
    }
}