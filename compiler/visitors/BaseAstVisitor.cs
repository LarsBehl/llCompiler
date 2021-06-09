using System;
using System.Globalization;
using System.Collections.Generic;
using LL.AST;
using LL.Types;
using LL.Exceptions;

namespace LL
{
    public partial class BuildAstVisitor : llBaseVisitor<IAST>
    {
        static VarExpr sR = null;
        private ProgramNode RootProgram;
        private Dictionary<string, IAST> Env;
        
        private BuildAstVisitor(): base()
        {

        }

        public BuildAstVisitor(string currentFile): this(new ProgramNode(currentFile, -1, -1))
        {

        }

        public BuildAstVisitor(ProgramNode rootProgram): this()
        {
            this.RootProgram = rootProgram;
            IAST.CurrentFile = rootProgram.FileName;
        }

        public override IAST VisitCompileUnit(llParser.CompileUnitContext context)
        {
            return Visit(context.program());
        }

        public override IAST VisitLine(llParser.LineContext context)
        {
            if (context.expression() != null)
                return Visit(context.expression());
            if (context.statement() != null)
                return Visit(context.statement());
            
            throw new NodeNotImplementedException(context.GetText(), this.RootProgram.FileName, context.Start.Line, context.Start.Column);
        }

        public override IAST VisitCompositUnit(llParser.CompositUnitContext context)
        {
            if (context.statement() != null)
                return Visit(context.statement());

            if (context.expression() != null)
                return Visit(context.expression());

            throw new NodeNotImplementedException(context.GetText(), this.RootProgram.FileName, context.Start.Line, context.Start.Column);
        }

        public override IAST VisitParenthes(llParser.ParenthesContext context)
        {
            return Visit(context.expression());
        }

        // this method should NEVER get called
        public override IAST VisitFunctionDefinition(llParser.FunctionDefinitionContext context)
        {
            throw new UnexpectedErrorException(this.RootProgram.FileName, context.Start.Line, context.Start.Column);
        }

        private IAST VisitFunctionDefinition(llParser.FunctionDefinitionContext context, FunctionDefinition funDef)
        {
            this.Env = funDef.FunctionEnv;
            int line = context.Start.Line;
            int column = context.Start.Column;

            // save the new function definition
            var body = Visit(context.body) as BlockStatement;

            if ((body.Type != funDef.ReturnType || !body.DoesFullyReturn)
            && !(funDef.ReturnType is VoidType))
            {
                if (body.Type is BlockStatementType || !body.DoesFullyReturn)
                    throw new MissingReturnStatementException(funDef.Name, funDef.ReturnType.ToString(), this.RootProgram.FileName, line, column);
                throw new TypeMissmatchException(funDef.ReturnType.ToString(), body.Type.ToString(), this.RootProgram.FileName, line, column);
            }

            if ((funDef.ReturnType is VoidType) && (!(body.Type is VoidType) && !(body.Type is BlockStatementType)))
                throw new TypeMissmatchException(funDef.ReturnType.ToString(), body.Type.ToString(), this.RootProgram.FileName, line, column);

            funDef.Body = body;

            return funDef;
        }

        public override IAST VisitProgram(llParser.ProgramContext context)
        {
            var structs = context.structDefinition();
            var funs = context.functionDefinition();
            var globalVariables = context.globalVariableStatement();

            foreach(var globalVariable in globalVariables)
                this.VisitGlobalVariableStatement(globalVariable);

            if(structs?.Length > 0)
            {
                foreach(var structDef in structs)
                    this.VisitStructDefinition(structDef, this.RootProgram.StructDefs[structDef.WORD().GetText()]);
            }

            if(funs?.Length > 0)
            {
                foreach(var fun in funs)
                    this.VisitFunctionDefinition(fun, this.RootProgram.FunDefs[fun.name.Text]);
            }

            if(context.compositUnit() != null)
            {
                this.Env = this.RootProgram.Env;
                this.RootProgram.CompositUnit = Visit(context.compositUnit());;
            }

            return this.RootProgram;
        }

        // this method should NEVER get called
        public override IAST VisitStructDefinition(llParser.StructDefinitionContext context)
        {
            throw new UnexpectedErrorException(this.RootProgram.FileName, context.Start.Line, context.Start.Column);
        }

        private IAST VisitStructDefinition(llParser.StructDefinitionContext context, StructDefinition structDefinition)
        {
            var props = context.structProperties();
            List<StructProperty> properties = new List<StructProperty>();

            foreach (var prop in props)
            {
                var tmp = Visit(prop) as StructProperty;

                if (properties.FindIndex(s => s.Name == tmp.Name) >= 0)
                    throw new PropertyAlreadyDefinedException(tmp.Name, context.WORD().GetText(), this.RootProgram.FileName, tmp.Line, tmp.Column);

                properties.Add(tmp);
            }

            structDefinition.Properties = properties;

            return structDefinition;
        }

        public override IAST VisitStructProperties(llParser.StructPropertiesContext context)
        {
            return new StructProperty(context.WORD().GetText(), Visit(context.typeDefinition()).Type, context.Start.Line, context.Start.Column);
        }
    }
}