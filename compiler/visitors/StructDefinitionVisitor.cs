using ll.AST;
using System;
using System.Collections.Generic;

namespace ll
{
    public class StructDefinitionVisitor : llBaseVisitor<IAST>
    {
        public override IAST VisitStructDefinition(llParser.StructDefinitionContext context)
        {
            string name = context.WORD().GetText();

            IAST.structs[name] = new StructDefinition(name, context.Start.Line, context.Start.Column);

            // unused value
            return null;
        }
    }
}