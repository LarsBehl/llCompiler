using ll.AST;
using System;
using System.Collections.Generic;

namespace ll
{
    public class FunctionDefinitionVisitor : llBaseVisitor<IAST>
    {
        // TODO rework grammar to eliminate repetitions
        public override IAST VisitFunctionDefinition(llParser.FunctionDefinitionContext context)
        {
            var identifier = context.WORD();

            if (IAST.funs.ContainsKey(identifier[0].GetText()))
                throw new ArgumentException($"Multiple definitions of \"{identifier[0].GetText()}\"");

            var types = context.typeDefinition();
            List<InstantiationStatement> args = new List<InstantiationStatement>();
            var tmpEnv = new Dictionary<string, IAST>();

            for (int i = 0; i < types.Length - 1; i++)
            {
                var tmpType = VisitTypeDefinition(types[i]);
                tmpEnv[identifier[i + 1].GetText()] = tmpType;
                args.Add(new InstantiationStatement(identifier[i + 1].GetText(), tmpType.type));
            }

            // BuildAstVisitor should add the function body
            // therefor it should check if the body is null, if not throw exception
            FunctionDefinition func = new FunctionDefinition(identifier[0].GetText(), args, null, tmpEnv, Visit(types[types.Length - 1]).type);
            IAST.funs[identifier[0].GetText()] = func;
            // unused; the llBaseVisitor expects a type
            return null;
        }

        public override IAST VisitTypeDefinition(llParser.TypeDefinitionContext context)
        {
            if (context.INT_TYPE() != null)
                return new IntLit(null);
            if (context.DOUBLE_TYPE() != null)
                return new DoubleLit(null);
            if (context.BOOL_TYPE() != null)
                return new BoolLit(null);
            if (context.VOID_TYPE() != null)
                return new VoidLit();
            if (context.arrayTypes() != null)
                return Visit(context.arrayTypes());

            throw new ArgumentException("Unsupported type");
        }

        public override IAST VisitIntArrayType(llParser.IntArrayTypeContext context)
        {
            return new IntArray();
        }

        public override IAST VisitDoubleArrayType(llParser.DoubleArrayTypeContext context)
        {
            return new DoubleArray();
        }

        public override IAST VisitBoolArrayType(llParser.BoolArrayTypeContext context)
        {
            return new BoolArray();
        }
    }
}