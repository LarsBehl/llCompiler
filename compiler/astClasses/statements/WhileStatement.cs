using System;
using ll.type;

namespace ll.AST
{
    public class WhileStatement : IAST
    {
        public IAST condition { get; set; }
        public IAST body { get; set; }
        public bool doesFullyReturn { get; set; }

        public WhileStatement(IAST condition, IAST body, int line, int column) : base(GetType(body), line, column)
        {
            if (!(condition.type is BooleanType))
                throw new ArgumentException($"While-Condition type \"{condition.type.typeName}\" does not match boolean");

            this.condition = condition;
            this.body = body;

            this.doesFullyReturn = DoesFullyReturn();
        }

        private static type.Type GetType(IAST body)
        {
            if (!(body.type is BlockStatementType))
                return body.type;

            return new WhileStatementType();
        }

        private bool DoesFullyReturn()
        {
            if (this.condition is BoolLit)
            {
                var tmp = this.condition as BoolLit;

                if (!(this.body.type is BlockStatementType))
                    return tmp.value ?? false;
            }

            return false;
        }
    }
}