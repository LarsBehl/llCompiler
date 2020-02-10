using System;
using System.Collections.Generic;
using ll.type;

namespace ll.AST
{
    public class IfStatement : IAST
    {
        public IAST cond { get; set; }
        public IAST ifBody { get; set; }
        public IAST elseBody { get; set; }
        public bool doesFullyReturn { get; set; }
        public IfStatement(IAST cond, IAST ifBody, IAST elseBody, int line, int column) : base(GetType(ifBody, elseBody, line, column), line, column)
        {
            if (!(cond.type is BooleanType))
                throw new ArgumentException($"If-Condition type \"{cond.type.typeName}\" does not match boolean; On line {line}:{column}");

            this.cond = cond;
            this.ifBody = ifBody;
            this.elseBody = elseBody;

            this.doesFullyReturn = this.DoesFullyReturn();
        }

        // TODO rework evaluation of type of conditions and loops
        // currently it is possible that a non void function does not return. But this should never happen
        private static type.Type GetType(IAST ifBody, IAST elseBody, int line, int column)
        {
            if (elseBody != null && !(ifBody.type is BlockStatementType) && !(elseBody.type is BlockStatementType) && elseBody.type.typeName != ifBody.type.typeName)
                throw new ArgumentException($"Returntype missmatch in if-statement \"{ifBody.type.typeName}\" \"{elseBody.type.typeName}\"; On line {line}:{column}");

            if (!(ifBody.type is BlockStatementType))
            {
                return ifBody.type;
            }

            if (elseBody != null && !(elseBody.type is BlockStatementType))
                return elseBody.type;

            return new IfStatementType();
        }

        private bool DoesFullyReturn()
        {
            if (elseBody != null && !(elseBody.type is BlockStatementType) && !(ifBody.type is BlockStatementType))
                return true;

            if (cond is BoolLit)
            {
                var tmp = cond as BoolLit;

                return tmp.value ?? false;
            }

            return false;
        }
    }
}