using ll.AST;
using System;

namespace ll
{
    public class StructDefinitionVisitor : llBaseVisitor<IAST>
    {
        public override IAST VisitStructDefinition(llParser.StructDefinitionContext context)
        {
            throw new NotImplementedException();
        }

        public override IAST VisitTypeDefinition(llParser.TypeDefinitionContext context)
        {
            throw new NotImplementedException();
        }
    }
}