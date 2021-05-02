using LL.Types;
using System;
using System.Collections.Generic;

namespace LL.AST
{
    public class BlockStatement : IAST
    {
        public List<IAST> Body { get; set; }
        public bool DoesFullyReturn { get; set; }

        public BlockStatement(List<IAST> body, int line, int column) : base(GetType(body), line, column)
        {
            this.Body = body;
            this.DoesFullyReturn = isFullyReturning();
        }

        private static LL.Types.Type GetType(List<IAST> body)
        {
            LL.Types.Type result = new BlockStatementType();

            foreach (var comp in body)
            {
                if (comp is ReturnStatement)
                {
                    result = comp.Type;
                    break;
                }

                if (comp is IfStatement && !(comp.Type is IfStatementType))
                {
                    result = comp.Type;
                    break;
                }

                if (comp is WhileStatement && !(comp.Type is WhileStatementType))
                {
                    result = comp.Type;
                    break;
                }
            }

            return result;
        }

        private bool isFullyReturning()
        {
            foreach (var comp in Body)
            {
                if (comp is ReturnStatement)
                    return true;

                if (comp is IfStatement)
                {
                    var tmp = comp as IfStatement;

                    if (tmp.DoesFullyReturn)
                        return true;
                }

                if (comp is WhileStatement)
                {
                    var tmp = comp as WhileStatement;

                    if (tmp.DoesFullyReturn)
                        return true;
                }
            }

            return false;
        }
    }
}