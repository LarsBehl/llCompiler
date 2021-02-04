using System;
using System.Globalization;
using System.Collections.Generic;
using ll.AST;
using ll.type;

namespace ll
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
            FunctionDefinition funDef = IAST.funs[identifier[0].GetText()];
            IAST.env = funDef.functionEnv;
            // save the new function definition
            if (funDef.body != null)
                throw new ArgumentException($"Trying to override the body of function \"{identifier[0].GetText()}\"; On line {context.Start.Line}:{context.Start.Column}");
            var body = Visit(context.body) as BlockStatement;

            if ((body.type != funDef.returnType || !body.doesFullyReturn)
            && !(funDef.returnType is VoidType))
            {
                if (body.type is BlockStatementType || !body.doesFullyReturn)
                    throw new ArgumentException($"Missing return statement in \"{funDef.name}\"; On line {context.Start.Line}:{context.Start.Column}");
                throw new ArgumentException($"Return type \"{body.type.typeName}\" does not match \"{funDef.returnType.typeName}\"; On line {context.Start.Line}:{context.Start.Column}");
            }

            if ((funDef.returnType is VoidType) && (!(body.type is VoidType) && !(body.type is BlockStatementType)))
                throw new ArgumentException($"Could not return \"{body.type.typeName}\" in a void function; On line {context.Start.Line}:{context.Start.Column}");
            if ((funDef.returnType is StructType ft && body.type is StructType bt) && ft.structName != bt.structName)
                throw new ArgumentException($"Return type \"{bt.structName}\" does not match \"{ft.structName}\"; On line {context.Start.Line}:{context.Start.Column}");

            funDef.body = body;
            IAST.funs[funDef.name] = funDef;

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

            if (!IAST.structs.ContainsKey(name))
                throw new ArgumentException($"Unknown struct \"{name}\"; On line {context.WORD().Symbol.Line}:{context.WORD().Symbol.Column}");

            StructDefinition structDef = IAST.structs[name];

            if (structDef.properties != null)
                throw new ArgumentException($"Multiple definitions of struct \"{name}\"; On line {context.WORD().Symbol.Line}:{context.WORD().Symbol.Column}");

            var props = context.structProperties();
            List<StructProperty> properties = new List<StructProperty>();

            foreach (var prop in props)
            {
                var tmp = Visit(prop) as StructProperty;

                if (properties.FindIndex(s => s.name == tmp.name) >= 0)
                    throw new ArgumentException($"Multiple definitions of \"{tmp.name}\" in struct \"{name}\"; On line {context.Start.Line}:{context.Start.Column}");

                properties.Add(tmp);
            }

            structDef.properties = properties;

            return structDef;
        }

        public override IAST VisitStructProperties(llParser.StructPropertiesContext context)
        {
            return new StructProperty(context.WORD().GetText(), Visit(context.typeDefinition()).type, context.Start.Line, context.Start.Column);
        }
    }
}