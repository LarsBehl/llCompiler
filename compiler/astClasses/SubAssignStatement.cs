using System;
using ll.type;

namespace ll.AST
{
    public class SubAssignStatement : IAST
    {
        public VarExpr left { get; set; }
        public IAST right { get; set; }

        public SubAssignStatement(VarExpr left, IAST right): base(new SubAssignStatementType())
        {
            if(left.type.typeName != right.type.typeName)
                throw new ArgumentException($"Type of variable \"{left.type.typeName}\" does not match \"{right.type.typeName}\"");
            
            this.left = left;
            this.right = right;
        }
    }
}