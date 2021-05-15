using System;
using System.Collections.Generic;
using LL.Types;

namespace LL.AST
{
    public class IfStatement : IAST
    {
        public IAST Cond { get; set; }
        public IAST IfBody { get; set; }
        public IAST ElseBody { get; set; }
        public bool DoesFullyReturn { get; set; }

        public IfStatement(IAST cond, IAST ifBody, IAST elseBody, int line, int column) : base(GetType(ifBody, elseBody, line, column), line, column)
        {
            if (!(cond.Type is BooleanType))
                throw new ArgumentException($"If-Condition type \"{cond.Type.TypeName}\" does not match boolean; On line {line}:{column}");

            this.Cond = cond;
            this.IfBody = ifBody;
            this.ElseBody = elseBody;

            this.DoesFullyReturn = this.isFullyReturning();
        }

        private static Types.Type GetType(IAST ifBody, IAST elseBody, int line, int column)
        {
            if (elseBody != null && !(ifBody.Type is BlockStatementType) && !(elseBody.Type is BlockStatementType) && elseBody.Type != ifBody.Type)
                throw new ArgumentException($"Returntype missmatch in if-statement \"{ifBody.Type.TypeName}\" \"{elseBody.Type.TypeName}\"; On line {line}:{column}");

            if (!(ifBody.Type is BlockStatementType))
            {
                return ifBody.Type;
            }

            if (elseBody != null && !(elseBody.Type is BlockStatementType))
                return elseBody.Type;

            return new IfStatementType();
        }

        private bool isFullyReturning()
        {
            if (ElseBody != null && !(ElseBody.Type is BlockStatementType) && !(IfBody.Type is BlockStatementType))
                return true;

            if (Cond is BoolLit)
            {
                var tmp = Cond as BoolLit;

                return tmp.Value ?? false;
            }

            return false;
        }
    }
}