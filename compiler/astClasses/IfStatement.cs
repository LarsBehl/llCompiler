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
            this.cond = cond;
            this.ifBody = ifBody;
            this.elseBody = elseBody;
        }

        private static type.Type GetType(IAST ifBody, IAST elseBody)
        {
            throw new NotImplementedException();
        }
    }
}