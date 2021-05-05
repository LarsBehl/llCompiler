using System;
using System.Globalization;
using System.Collections.Generic;
using LL.AST;
using LL.Types;
using LL.Exceptions;

namespace LL
{
    // TODO add constructor that takes ProgramNode as argument
    public partial class BuildAstVisitor : llBaseVisitor<IAST>
    {
        private string CurrentFile;
        static VarExpr sR = null;
        
        private BuildAstVisitor(): base()
        {

        }

        public BuildAstVisitor(string currentFile): this() => this.CurrentFile = currentFile;

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
            
            throw new NodeNotImplementedException(context.GetText(), this.CurrentFile, context.Start.Line, context.Start.Column);
        }

        public override IAST VisitCompositUnit(llParser.CompositUnitContext context)
        {
            if (context.statement() != null)
                return Visit(context.statement());

            if (context.expression() != null)
                return Visit(context.expression());

            throw new NodeNotImplementedException(context.GetText(), this.CurrentFile, context.Start.Line, context.Start.Column);
        }

        public override IAST VisitParenthes(llParser.ParenthesContext context)
        {
            return Visit(context.expression());
        }

        // TODO reimplement
        // TODO add overload that takes a FunctionDefinition as an argument
        public override IAST VisitFunctionDefinition(llParser.FunctionDefinitionContext context)
        {
            // TODO add FatalErrorException as this method should never get called
            var identifier = context.WORD();
            FunctionDefinition funDef = IAST.Funs[identifier[0].GetText()];
            IAST.Env = funDef.FunctionEnv;
            int line = context.Start.Line;
            int column = context.Start.Column;

            // save the new function definition
            var body = Visit(context.body) as BlockStatement;

            if ((body.Type != funDef.ReturnType || !body.DoesFullyReturn)
            && !(funDef.ReturnType is VoidType))
            {
                if (body.Type is BlockStatementType || !body.DoesFullyReturn)
                    throw new MissingReturnStatementException(funDef.Name, funDef.ReturnType.ToString(), this.CurrentFile, line, column);
                throw new TypeMissmatchException(funDef.ReturnType.ToString(), body.Type.ToString(), this.CurrentFile, line, column);
            }

            if ((funDef.ReturnType is VoidType) && (!(body.Type is VoidType) && !(body.Type is BlockStatementType)))
                throw new TypeMissmatchException(funDef.ReturnType.ToString(), body.Type.ToString(), this.CurrentFile, line, column);

            funDef.Body = body;
            IAST.Funs[funDef.Name] = funDef;

            return funDef;
        }

        // TODO reimplement
        public override IAST VisitProgram(llParser.ProgramContext context)
        {
            // List<IAST> funDefs = new List<IAST>();
            // List<StructDefinition> structDefs = new List<StructDefinition>();
            // var structs = context.structDefinition();
            // var funs = context.functionDefinition();
            // int line = context.Start.Line;
            // int column = context.Start.Column;

            // if (funs?.Length > 0 || structs?.Length > 0)
            // {
            //     foreach (var structDef in context.structDefinition())
            //         structDefs.Add(Visit(structDef) as StructDefinition);

            //     foreach (var funDef in funs)
            //         funDefs.Add(Visit(funDef));

            //     return new ProgramNode(funDefs, structDefs, line, column);
            // }

            // if (context.compositUnit() != null)
            //     return Visit(context.compositUnit());

            // throw new NodeNotImplementedException(context.GetText(), this.CurrentFile, line, column);

            throw new NotImplementedException();
        }

        // TODO reimplement
        // TODO overload method so it takes a StructDefinition as an Argument
        public override IAST VisitStructDefinition(llParser.StructDefinitionContext context)
        {
            // TODO add FatalErrorException as this method should never get called
            string name = context.WORD().GetText();
            int line = context.WORD().Symbol.Line;
            int column = context.WORD().Symbol.Column;

            if (!IAST.Structs.ContainsKey(name))
                throw new UnknownTypeException(name, this.CurrentFile, line, column);

            StructDefinition structDef = IAST.Structs[name];

            var props = context.structProperties();
            List<StructProperty> properties = new List<StructProperty>();

            foreach (var prop in props)
            {
                var tmp = Visit(prop) as StructProperty;

                if (properties.FindIndex(s => s.Name == tmp.Name) >= 0)
                    throw new PropertyAlreadyDefinedException(tmp.Name, name, this.CurrentFile, line, column);

                properties.Add(tmp);
            }

            structDef.Properties = properties;

            return structDef;
        }

        public override IAST VisitStructProperties(llParser.StructPropertiesContext context)
        {
            return new StructProperty(context.WORD().GetText(), Visit(context.typeDefinition()).Type, context.Start.Line, context.Start.Column);
        }
    }
}