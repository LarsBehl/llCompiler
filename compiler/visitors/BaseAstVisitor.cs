using System;
using System.Globalization;
using System.Collections.Generic;
using LL.AST;
using LL.Types;

namespace LL
{
    public partial class BuildAstVisitor : llBaseVisitor<IAST>
    {
        static VarExpr sR = null;
        
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

            throw new ArgumentException($"Unknown AST Node; On line {context.Start.Line}:{context.Start.Column}");
        }

        public override IAST VisitCompositUnit(llParser.CompositUnitContext context)
        {
            if (context.statement() != null)
                return Visit(context.statement());

            if (context.expression() != null)
                return Visit(context.expression());

            throw new ArgumentException($"Unknown node; On line {context.Start.Line}:{context.Start.Column}");
        }

        public override IAST VisitParenthes(llParser.ParenthesContext context)
        {
            return Visit(context.expression());
        }

        public override IAST VisitFunctionDefinition(llParser.FunctionDefinitionContext context)
        {
            var identifier = context.WORD();
            FunctionDefinition funDef = IAST.Funs[identifier[0].GetText()];
            IAST.Env = funDef.FunctionEnv;
            // save the new function definition
            if (funDef.Body != null)
                throw new ArgumentException($"Trying to override the body of function \"{identifier[0].GetText()}\"; On line {context.Start.Line}:{context.Start.Column}");
            var body = Visit(context.body) as BlockStatement;

            if ((body.Type != funDef.ReturnType || !body.DoesFullyReturn)
            && !(funDef.ReturnType is VoidType))
            {
                if (body.Type is BlockStatementType || !body.DoesFullyReturn)
                    throw new ArgumentException($"Missing return statement in \"{funDef.Name}\"; On line {context.Start.Line}:{context.Start.Column}");
                throw new ArgumentException($"Return type \"{body.Type.typeName}\" does not match \"{funDef.ReturnType.typeName}\"; On line {context.Start.Line}:{context.Start.Column}");
            }

            if ((funDef.ReturnType is VoidType) && (!(body.Type is VoidType) && !(body.Type is BlockStatementType)))
                throw new ArgumentException($"Could not return \"{body.Type.typeName}\" in a void function; On line {context.Start.Line}:{context.Start.Column}");
            if ((funDef.ReturnType is StructType ft && body.Type is StructType bt) && ft.structName != bt.structName)
                throw new ArgumentException($"Return type \"{bt.structName}\" does not match \"{ft.structName}\"; On line {context.Start.Line}:{context.Start.Column}");

            funDef.Body = body;
            IAST.Funs[funDef.Name] = funDef;

            return funDef;
        }

        public override IAST VisitProgram(llParser.ProgramContext context)
        {
            List<IAST> funDefs = new List<IAST>();
            List<IAST> structDefs = new List<IAST>();
            var structs = context.structDefinition();
            var funs = context.functionDefinition();

            if (funs?.Length > 0 || structs?.Length > 0)
            {
                foreach (var structDef in context.structDefinition())
                    structDefs.Add(Visit(structDef));

                foreach (var funDef in funs)
                    funDefs.Add(Visit(funDef));

                return new ProgramNode(funDefs, structDefs, context.Start.Line, context.Start.Column);
            }

            if (context.compositUnit() != null)
                return Visit(context.compositUnit());

            throw new ArgumentException($"Unknown node in Program; On line {context.Start.Line}:{context.Start.Column}");
        }

        public override IAST VisitStructDefinition(llParser.StructDefinitionContext context)
        {
            string name = context.WORD().GetText();

            if (!IAST.Structs.ContainsKey(name))
                throw new ArgumentException($"Unknown struct \"{name}\"; On line {context.WORD().Symbol.Line}:{context.WORD().Symbol.Column}");

            StructDefinition structDef = IAST.Structs[name];

            if (structDef.Properties != null)
                throw new ArgumentException($"Multiple definitions of struct \"{name}\"; On line {context.WORD().Symbol.Line}:{context.WORD().Symbol.Column}");

            var props = context.structProperties();
            List<StructProperty> properties = new List<StructProperty>();

            foreach (var prop in props)
            {
                var tmp = Visit(prop) as StructProperty;

                if (properties.FindIndex(s => s.Name == tmp.Name) >= 0)
                    throw new ArgumentException($"Multiple definitions of \"{tmp.Name}\" in struct \"{name}\"; On line {context.Start.Line}:{context.Start.Column}");

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