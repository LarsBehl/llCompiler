using ll.type;
using System.Collections.Generic;

namespace ll.AST
{
    public class ExpressionSequenz : IAST
    {
        public List<IAST> body { get; set; }

        public ExpressionSequenz(List<IAST> body) : base(new ExpressionSequenzType())
        {
            this.body = body;
        }
    }
}