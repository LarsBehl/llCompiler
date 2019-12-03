using ll.type;
using System;
using System.Collections.Generic;

namespace ll.AST
{
    public class BlockStatement : IAST
    {
        public List<IAST> body { get; set; }

        public BlockStatement(List<IAST> body) : base(GetType(body))
        {
            this.body = body;
        }

        private static ll.type.Type GetType(List<IAST> body)
        {
            ll.type.Type result = new BlockStatementType();

            foreach (var comp in body)
            {
                if (comp is ReturnStatement)
                {
                    result = comp.type;
                    break;
                }

                if(comp is IfStatement && !(comp.type is IfStatementType))
                {
                    result = comp.type;
                    break;
                }
            }

            return result;
        }
    }
}