using System;
using LL.AST;
using LL.Exceptions;

namespace LL
{
    public partial class BuildAstVisitor : llBaseVisitor<IAST>
    {
        public override IAST VisitTypeDefinition(llParser.TypeDefinitionContext context)
        {
            int line = context.Start.Line;
            int column = context.Start.Column;

            if (context.INT_TYPE() != null)
                return new IntLit(null, line, column);
            if (context.DOUBLE_TYPE() != null)
                return new DoubleLit(null, line, column);
            if (context.BOOL_TYPE() != null)
                return new BoolLit(null, line, column);
            if (context.VOID_TYPE() != null)
                return new VoidLit(line, column);
            if (context.arrayTypes() != null)
                return Visit(context.arrayTypes());
            if (context.structName() != null)
                return Visit(context.structName());
            
            throw new UnknownTypeException(context.GetText(), this.RootProgram.FileName, line, column);
        }

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
            int line = context.Start.Line;
            int column = context.Start.Column;

            if (!this.RootProgram.ContainsStruct(name))
                throw new UnknownTypeException(name, this.RootProgram.FileName, line, column);

            return new Struct(name, line, column);
        }
    }
}