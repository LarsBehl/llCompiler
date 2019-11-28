using ll.type;
using System.Collections.Generic;

namespace ll.AST
{
    public class BlockStatement : IAST
    {
        public List<IAST> body { get; set; }

        public BlockStatement(List<IAST> body) : base(new ExpressionSequenzType())
        {
            this.body = body;
        }
    }
}