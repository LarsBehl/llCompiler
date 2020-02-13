using ll.type;
using System;
using System.Collections.Generic;

namespace ll.AST
{
    public class BlockStatement : IAST
    {
        public List<IAST> body { get; set; }
        public bool doesFullyReturn { get; set; }

        public BlockStatement(List<IAST> body, int line, int column) : base(GetType(body), line, column)
        {
            this.body = body;
            this.doesFullyReturn = DoesFullyReturn();
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

                if (comp is IfStatement && !(comp.type is IfStatementType))
                {
                    result = comp.type;
                    break;
                }

                if (comp is WhileStatement && !(comp.type is WhileStatementType))
                {
                    result = comp.type;
                    break;
                }
            }

            return result;
        }

        private bool DoesFullyReturn()
        {
            foreach (var comp in body)
            {
                if (comp is ReturnStatement)
                    return true;

                if (comp is IfStatement)
                {
                    var tmp = comp as IfStatement;

                    if (tmp.doesFullyReturn)
                        return true;
                }

                if (comp is WhileStatement)
                {
                    var tmp = comp as WhileStatement;

                    if (tmp.doesFullyReturn)
                        return true;
                }
            }

            return false;
        }
    }
}