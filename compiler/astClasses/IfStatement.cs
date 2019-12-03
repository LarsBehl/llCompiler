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
        public IfStatement(IAST cond, IAST ifBody, IAST elseBody): base(GetType(ifBody, elseBody))
        {
            if(!(cond.type is BooleanType))
                throw new ArgumentException($"If-Condition type \"{cond.type.typeName}\" does not match boolean");
            this.cond = cond;
            this.ifBody = ifBody;
            this.elseBody = elseBody;
        }

        private static type.Type GetType(IAST ifBody, IAST elseBody)
        {
            if(!(ifBody.type is BlockStatementType))
                return ifBody.type;

            if(!(elseBody?.type is BlockStatementType))
                return elseBody.type;

            return new IfStatementType();
        }
    }
}