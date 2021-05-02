using LL.AST;
using System;
using System.Collections.Generic;

namespace LL
{
    public class FunctionDefinitionVisitor : llBaseVisitor<IAST>
    {
        public override IAST VisitFunctionDefinition(llParser.FunctionDefinitionContext context)
        {
            var identifier = context.WORD();

            if (IAST.Funs.ContainsKey(identifier[0].GetText()))
                throw new ArgumentException($"Multiple definitions of \"{identifier[0].GetText()}\"; On line {context.Start.Line}:{context.Start.Column}");

            var types = context.typeDefinition();
            List<InstantiationStatement> args = new List<InstantiationStatement>();
            var tmpEnv = new Dictionary<string, IAST>();

            for (int i = 0; i < types.Length - 1; i++)
            {
                var tmpType = VisitTypeDefinition(types[i]);
                tmpEnv[identifier[i + 1].GetText()] = tmpType;
                args.Add(new InstantiationStatement(identifier[i + 1].GetText(), tmpType.Type, tmpType.Line, tmpType.Column));
            }

            // BuildAstVisitor should add the function body
            // therefor it should check if the body is null, if not throw exception
            FunctionDefinition func = new FunctionDefinition(identifier[0].GetText(), args, null, tmpEnv, Visit(types[types.Length - 1]).Type, context.Start.Line, context.Start.Column);
            IAST.Funs[identifier[0].GetText()] = func;
            // unused; the llBaseVisitor expects a type
            return null;
        }

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
            StructDefinition structDef;

            try
            {
                structDef = IAST.Structs[context.WORD().GetText()];
            }
            catch (Exception)
            {
                throw new ArgumentException($"Unknown struct reference \"{context.WORD().GetText()}\"; On line {context.Start.Line}:{context.Start.Column}");
            }

            return new Struct(structDef.Name, context.Start.Line, context.Start.Column);
        }
    }
}