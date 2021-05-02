using System;
using LL.Types;

namespace LL.AST
{
    public class WhileStatement : IAST
    {
        public IAST Condition { get; set; }
        public IAST Body { get; set; }
        public bool DoesFullyReturn { get; set; }

        public WhileStatement(IAST condition, IAST body, int line, int column) : base(GetType(body), line, column)
        {
            if (!(condition.Type is BooleanType))
                throw new ArgumentException($"While-Condition type \"{condition.Type.typeName}\" does not match boolean; On line {line}:{column}");

            this.Condition = condition;
            this.Body = body;

            this.DoesFullyReturn = IsFullyReturning();
        }

        private static Types.Type GetType(IAST body)
        {
            if (!(body.Type is BlockStatementType))
                return body.Type;

            return new WhileStatementType();
        }

        private bool IsFullyReturning()
        {
            if (this.Condition is BoolLit)
            {
                var tmp = this.Condition as BoolLit;

                if (!(this.Body.Type is BlockStatementType))
                    return tmp.Value ?? false;
            }

            return false;
        }
    }
}