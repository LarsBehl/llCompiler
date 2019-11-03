using System.Collections.Generic;

namespace ll.AST
{
    public class ExpressionSequenz : IAST
    {
        public List<IAST> body { get; set; }

        public ExpressionSequenz(List<IAST> body)
        {
            this.body = body;
        }
    }
}