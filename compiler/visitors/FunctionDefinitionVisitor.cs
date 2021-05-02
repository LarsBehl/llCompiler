using LL.AST;
using LL.Exceptions;
using System;
using System.Collections.Generic;

namespace LL
{
    public class FunctionDefinitionVisitor : llBaseVisitor<IAST>
    {
        private string CurrentFile;

        private FunctionDefinitionVisitor(): base()
        {

        }

        public FunctionDefinitionVisitor(string currentFile): this()
        {
            this.CurrentFile = currentFile;
        }

        public override IAST VisitFunctionDefinition(llParser.FunctionDefinitionContext context)
        {
            var identifier = context.WORD();
            int line = context.Start.Line;
            int column = context.Start.Column;

            if (IAST.Funs.ContainsKey(identifier[0].GetText()))
                throw new FunctionAlreadyDefinedException(identifier[0].GetText(), this.CurrentFile, line, column);

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
            FunctionDefinition func = new FunctionDefinition(identifier[0].GetText(), args, null, tmpEnv, Visit(types[types.Length - 1]).Type, line, column);
            IAST.Funs[identifier[0].GetText()] = func;
            // unused; the llBaseVisitor expects a type
            return null;
        }

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

            throw new UnknownTypeException(context.GetText(), this.CurrentFile, line, column);
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
            string structName = context.WORD().GetText();
            int line = context.Start.Line;
            int column = context.Start.Column;

            try
            {
                structDef = IAST.Structs[structName];
            }
            catch (Exception)
            {
                throw new UnknownTypeException(structName, this.CurrentFile, line, column);
            }

            return new Struct(structDef.Name, line, column);
        }
    }
}