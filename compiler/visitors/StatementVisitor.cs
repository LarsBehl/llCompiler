using System;
using System.Collections.Generic;
using LL.AST;
using LL.Types;

namespace LL
{
    public partial class BuildAstVisitor : llBaseVisitor<IAST>
    {
        public override IAST VisitAssignStatement(llParser.AssignStatementContext context)
        {
            IAST right;

            if (!IAST.Env.ContainsKey(context.left.Text))
                throw new ArgumentException($"Unknown variable \"{context.left.Text}\"; On line {context.left.Line}:{context.left.Column}");

            // check if righthand side of the assignment is an array or an expression
            if (context.expression() != null)
                right = Visit(context.expression());
            else
                right = Visit(context.refTypeCreation());

            return new AssignStatement(new VarExpr(context.left.Text, context.Start.Line, context.left.StartIndex), right, context.Start.Line, context.Start.Column);
        }

        public override IAST VisitBlockStatement(llParser.BlockStatementContext context)
        {
            List<IAST> body = new List<IAST>();
            var tmp = context.line();

            foreach (var comp in tmp)
            {
                var compVisited = Visit(comp);
                body.Add(compVisited);
                if (compVisited is ReturnStatement)
                    break;
            }

            return new BlockStatement(body, context.Start.Line, context.Start.Column);
        }

        public override IAST VisitReturnStatement(llParser.ReturnStatementContext context)
        {
            if (context.expression() != null)
                return new ReturnStatement(Visit(context.expression()), context.Start.Line, context.Start.Column);
            if (context.refTypeCreation() != null)
                return new ReturnStatement(Visit(context.refTypeCreation()), context.Start.Line, context.Start.Column);

            return new ReturnStatement(context.Start.Line, context.Start.Column);
        }

        public override IAST VisitInitializationStatement(llParser.InitializationStatementContext context)
        {
            IAST variable = Visit(context.type);

            if (variable.Type is VoidType)
                throw new ArgumentException($"Type \"{variable.Type.typeName}\" not allowed for variables; On line {variable.Line}:{variable.Column}");

            IAST.Env[context.left.Text] = variable;

            IAST val;
            if (context.expression() != null)
                val = Visit(context.expression());
            else
                val = Visit(context.refTypeCreation());

            if (variable.Type != val.Type)
            {
                if (variable.Type is not DoubleType || val.Type is not IntType)
                    throw new ArgumentException($"Type \"{val.Type.typeName}\" does not match \"{variable.Type.typeName}\"; On line {context.Start.Line}:{context.Start.Column}");
            }

            return new AssignStatement(new VarExpr(context.left.Text, context.Start.Line, context.left.StartIndex), val, context.Start.Line, context.Start.Column);
        }

        public override IAST VisitInstantiationStatement(llParser.InstantiationStatementContext context)
        {
            IAST variable = Visit(context.type);

            if (variable.Type is VoidType)
                throw new ArgumentException($"Type \"{variable.Type.typeName}\" is not allowed for variables; On line {variable.Line}:{variable.Column}");

            IAST.Env[context.left.Text] = variable;

            return new InstantiationStatement(context.WORD().GetText(), variable.Type, context.Start.Line, context.Start.Column);
        }

        public override IAST VisitIfStatement(llParser.IfStatementContext context)
        {
            var tmp = context.blockStatement();
            var cond = Visit(context.cond);
            var ifBody = Visit(tmp[0]);
            IAST elseBody = null;

            if (tmp.Length > 1)
                elseBody = Visit(tmp[1]);

            return new IfStatement(cond, ifBody, elseBody, context.Start.Line, context.Start.Column);
        }

        public override IAST VisitWhileStatement(llParser.WhileStatementContext context)
        {
            var cond = Visit(context.cond);
            var body = Visit(context.blockStatement());

            return new WhileStatement(cond, body, context.Start.Line, context.Start.Column);
        }

        public override IAST VisitAddAssignStatement(llParser.AddAssignStatementContext context)
        {
            if (!IAST.Env.ContainsKey(context.left.Text))
                throw new ArgumentException($"Unknown variable \"{context.left.Text}\"; On line {context.Start.Line}:{context.Start.Column}");

            return new AddAssignStatement(new VarExpr(context.left.Text, context.left.Line, context.left.Column), Visit(context.right), context.Start.Line, context.Start.Column);
        }

        public override IAST VisitSubAssignStatement(llParser.SubAssignStatementContext context)
        {
            if (!IAST.Env.ContainsKey(context.left.Text))
                throw new ArgumentException($"Unknown variable \"{context.left.Text}\"; On line {context.Start.Line}:{context.Start.Column}");

            return new SubAssignStatement(new VarExpr(context.left.Text, context.left.Line, context.left.Column), Visit(context.right), context.Start.Line, context.Start.Column);
        }

        public override IAST VisitMultAssignStatement(llParser.MultAssignStatementContext context)
        {
            if (!IAST.Env.ContainsKey(context.left.Text))
                throw new ArgumentException($"Unknown variable {context.left.Text}; On line {context.Start.Line}:{context.Start.Column}");

            return new MultAssignStatement(new VarExpr(context.left.Text, context.left.Line, context.left.Column), Visit(context.right), context.Start.Line, context.Start.Column);
        }

        public override IAST VisitDivAssignStatement(llParser.DivAssignStatementContext context)
        {
            if (!IAST.Env.ContainsKey(context.left.Text))
                throw new ArgumentException($"Unknown variable {context.left.Text}; On line {context.Start.Line}:{context.Start.Column}");

            return new DivAssignStatement(new VarExpr(context.left.Text, context.left.Line, context.left.Column), Visit(context.right), context.Start.Line, context.Start.Column);
        }

        public override IAST VisitPrintStatement(llParser.PrintStatementContext context)
        {
            return new PrintStatement(Visit(context.expression()), context.Start.Line, context.Start.Column);
        }

        public override IAST VisitIntArrayCreation(llParser.IntArrayCreationContext context)
        {
            return new IntArray(Visit(context.expression()), context.Start.Line, context.Start.Column);
        }

        public override IAST VisitDoubleArrayCreation(llParser.DoubleArrayCreationContext context)
        {
            return new DoubleArray(Visit(context.expression()), context.Start.Line, context.Start.Column);
        }

        public override IAST VisitBoolArrayCreation(llParser.BoolArrayCreationContext context)
        {
            return new BoolArray(Visit(context.expression()), context.Start.Line, context.Start.Column);
        }

        public override IAST VisitRefTypeCreation(llParser.RefTypeCreationContext context)
        {
            if (context.arrayCreation() != null)
                return new RefTypeCreationStatement(Visit(context.arrayCreation()), context.Start.Line, context.Start.Column);
            if (context.structCreation() != null)
                return new RefTypeCreationStatement(Visit(context.structCreation()), context.Start.Line, context.Start.Column);

            throw new ArgumentException($"Invalid type for reference type creation; On line {context.Start.Line}:{context.Start.Column}");
        }

        public override IAST VisitAssignArrayField(llParser.AssignArrayFieldContext context)
        {
            ArrayIndexing arrayIndexing = Visit(context.arrayIndexing()) as ArrayIndexing;

            return new AssignArrayField(arrayIndexing, Visit(context.expression()), context.Start.Line, context.Start.Column);
        }

        public override IAST VisitDestructionStatement(llParser.DestructionStatementContext context)
        {
            return Visit(context.refTypeDestruction());
        }

        public override IAST VisitRefTypeDestruction(llParser.RefTypeDestructionContext context)
        {
            return new DestructionStatement(Visit(context.valueAccess()) as ValueAccessExpression, context.Start.Line, context.Start.Column);
        }

        public override IAST VisitStructCreation(llParser.StructCreationContext context)
        {
            return Visit(context.structName());
        }

        public override IAST VisitAssignStructProp(llParser.AssignStructPropContext context)
        {
            StructPropertyAccess structPropAccess = Visit(context.structPropertyAccess()) as StructPropertyAccess;
            IAST val;

            if (context.expression() != null)
                val = Visit(context.expression());
            else
                val = Visit(context.refTypeCreation());

            return new AssignStructProperty(structPropAccess, val, context.Start.Line, context.Start.Column);
        }
    }
}