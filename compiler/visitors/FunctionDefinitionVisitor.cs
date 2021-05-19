using System.Collections.Generic;

using Antlr4.Runtime.Misc;
using Antlr4.Runtime.Tree;

using LL.AST;
using LL.Exceptions;

namespace LL
{
    public class FunctionDefinitionVisitor : llBaseVisitor<IAST>
    {
        private string CurrentFile;
        private ProgramNode RootProg;

        private FunctionDefinitionVisitor() : base()
        {

        }

        public FunctionDefinitionVisitor(string currentFile) : this(currentFile, new ProgramNode(currentFile, -1, -1))
        {

        }

        public FunctionDefinitionVisitor(string currentFile, ProgramNode rootProg) : this()
        {
            this.CurrentFile = currentFile;
            this.RootProg = rootProg;
        }

        public FunctionDefinitionVisitor(ProgramNode rootProg) : this(rootProg.FileName, rootProg)
        {

        }

        public override IAST VisitCompileUnit([NotNull] llParser.CompileUnitContext context)
        {
            return this.Visit(context.program());
        }

        public override IAST VisitProgram([NotNull] llParser.ProgramContext context)
        {
            var funs = context.functionDefinition();
            foreach (var fun in funs)
                this.Visit(fun);

            var funProtos = context.functionPrototype();
            foreach (var funProto in funProtos)
                this.Visit(funProto);

            return this.RootProg;
        }

        public override IAST VisitFunctionDefinition(llParser.FunctionDefinitionContext context)
        {
            this.AddFunctionDefinition(context.name.Text, context.typeDefinition(), context.WORD(), context.Start.Line, context.Start.Column);

            // unused value
            return null;
        }

        public override IAST VisitFunctionPrototype([NotNull] llParser.FunctionPrototypeContext context)
        {
            this.AddFunctionDefinition(context.name.Text, context.typeDefinition(), context.WORD(), context.Start.Line, context.Start.Column);

            // unused value
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
            string structName = context.WORD().GetText();
            int line = context.Start.Line;
            int column = context.Start.Column;

            bool success = this.RootProg.ContainsStruct(structName);

            if (!success)
                throw new UnknownTypeException(structName, this.CurrentFile, line, column);

            return new Struct(structName, line, column);
        }

        private void AddFunctionDefinition(string functionName, llParser.TypeDefinitionContext[] types, ITerminalNode[] identifiers, int line, int column)
        {
            if (this.RootProg.IsFunctionDefined(functionName))
                throw new FunctionAlreadyDefinedException(functionName, this.CurrentFile, line, column);

            List<InstantiationStatement> args = new List<InstantiationStatement>();
            Dictionary<string, IAST> tmpEnv = new Dictionary<string, IAST>();

            for (int i = 0; i < types.Length - 1; i++)
            {
                IAST tmpType = VisitTypeDefinition(types[i]);
                string argName = identifiers[i + 1].GetText();
                tmpEnv[argName] = tmpType;
                args.Add(new InstantiationStatement(argName, tmpType.Type, tmpType.Line, tmpType.Column));
            }

            this.RootProg.FunDefs.Add(functionName, new FunctionDefinition(functionName, args, tmpEnv, Visit(types[types.Length - 1]).Type, line, column));
        }
    }
}