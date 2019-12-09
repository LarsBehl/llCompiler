using System;
using ll.type;

namespace ll.AST
{
    public class WhileStatement : IAST {
        public IAST condition { get; set; }
        public IAST body { get; set; }
        
        public WhileStatement(IAST condition, IAST body): base(GetType(body))
        {
            if(!(condition.type is BooleanType))
                throw new ArgumentException($"While-Condition type \"{condition.type.typeName}\" does not match boolean");

            this.condition = condition;
            this.body = body;
        }

        private static type.Type GetType(IAST body)
        {
            if(!(body.type is BlockStatementType))
                return body.type;
            
            return new WhileStatementType();
        }
    }
}