using System;
using LL.AST;

namespace LL
{
    public partial class BuildAstVisitor : llBaseVisitor<IAST>
    {
        public override IAST VisitTypeDefinition(llParser.TypeDefinitionContext context)
        {
            if (context.INT_TYPE() != null)
                return new IntLit(null, context.Start.Line, context.Start.Column);
            if (context.DOUBLE_TYPE() != null)
                return new DoubleLit(null, context.Start.Line, context.Start.Column);
            if (context.BOOL_TYPE() != null)
                return new BoolLit(null, context.Start.Line, context.Start.Column);
            if (context.VOID_TYPE() != null)
                return new VoidLit(context.Start.Line, context.Start.Column);
            if (context.arrayTypes() != null)
                return Visit(context.arrayTypes());
            if (context.structName() != null)
                return Visit(context.structName());
            throw new ArgumentException($"Unsupported type; On line {context.Start.Line}:{context.Start.Column}");
        }

        // TODO rework arrays so it is possible to create arrays of reference types
        public override IAST VisitIntArrayType(llParser.IntArrayTypeContext context)
        {
            return new IntArray(context.Start.Line, context.Start.Column);
        }

        public override IAST VisitDoubleArrayType(llParser.DoubleArrayTypeContext context)
        {
            return new DoubleArray(context.Start.Line, context.Start.Column);
        }

        public override IAST VisitBoolArrayType(llParser.BoolArrayTypeContext context)
        {
            return new BoolArray(context.Start.Line, context.Start.Column);
        }

        public override IAST VisitStructName(llParser.StructNameContext context)
        {
            string name = context.WORD().GetText();

            if (!IAST.Structs.ContainsKey(name))
                throw new ArgumentException($"Unknown struct \"{name}\"; On line {context.Start.Line}:{context.Start.Column}");

            return new Struct(name, context.Start.Line, context.Start.Column);
        }
    }
}