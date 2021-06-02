using System;
using System.Collections.Generic;
using LL.AST;
using LL.Types;
using LL.Exceptions;
using Antlr4.Runtime.Misc;

namespace LL
{
    public partial class BuildAstVisitor : llBaseVisitor<IAST>
    {
        public override IAST VisitAssignStatement(llParser.AssignStatementContext context)
        {
            IAST right;
            string name = context.left.Text;
            int line = context.Start.Line;
            int column = context.Start.Column;

            if (!this.Env.ContainsKey(name))
                throw new UnknownVariableException(name, this.RootProgram.FileName, line, column);

            // check if righthand side of the assignment is an array or an expression
            if (context.expression() != null)
                right = Visit(context.expression());
            else
                right = Visit(context.refTypeCreation());

            return new AssignStatement(new VarExpr(name, this.TryGetType(name, line, column), line, column), right, line, context.Start.Column);
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
            int line = context.Start.Line;
            int column = context.Start.Column;
            string name = context.left.Text;

            if (variable.Type is VoidType)
                throw new TypeNotAllowedException(variable.Type.ToString(), this.RootProgram.FileName, variable.Line, variable.Column);

            this.Env[name] = variable;

            IAST val;
            if (context.expression() != null)
                val = Visit(context.expression());
            else
                val = Visit(context.refTypeCreation());

            if (variable.Type != val.Type)
            {
                if (variable.Type is not DoubleType || val.Type is not IntType)
                    throw new TypeMissmatchException(variable.Type.ToString(), val.Type.ToString(), this.RootProgram.FileName, line, column);
            }

            return new AssignStatement(new VarExpr(name, variable.Type, context.left.Line, context.left.StartIndex), val, line, column);
        }

        public override IAST VisitInstantiationStatement(llParser.InstantiationStatementContext context)
        {
            IAST variable = Visit(context.type);
            string variableName = context.left.Text;
            int line = context.Start.Line;
            int column = context.Start.Column;

            if (variable.Type is VoidType)
                throw new TypeNotAllowedException(variable.Type.ToString(), this.RootProgram.FileName, variable.Line, variable.Column);

            if (this.Env.ContainsKey(variableName))
                throw new VariableAlreadyDefinedException(variableName, this.RootProgram.FileName, line, column);

            this.Env[variableName] = variable;

            return new InstantiationStatement(context.WORD().GetText(), variable.Type, line, column);
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
            string variableName = context.left.Text;
            int line = context.Start.Line;
            int column = context.Start.Column;

            if (!this.Env.ContainsKey(variableName))
                throw new UnknownVariableException(variableName, this.RootProgram.FileName, line, column);

            return new AddAssignStatement(new VarExpr(variableName, this.TryGetType(variableName, line, column), context.left.Line, context.left.Column), Visit(context.right), line, column);
        }

        public override IAST VisitSubAssignStatement(llParser.SubAssignStatementContext context)
        {
            string variableName = context.left.Text;
            int line = context.Start.Line;
            int column = context.Start.Column;

            if (!this.Env.ContainsKey(variableName))
                throw new UnknownVariableException(variableName, this.RootProgram.FileName, line, column);

            return new SubAssignStatement(new VarExpr(variableName, this.TryGetType(variableName, line, column), context.left.Line, context.left.Column), Visit(context.right), line, column);
        }

        public override IAST VisitMultAssignStatement(llParser.MultAssignStatementContext context)
        {
            string variableName = context.left.Text;
            int line = context.Start.Line;
            int column = context.Start.Column;

            if (!this.Env.ContainsKey(variableName))
                throw new UnknownVariableException(variableName, this.RootProgram.FileName, line, column);

            return new MultAssignStatement(new VarExpr(variableName, this.TryGetType(variableName, line, column), context.left.Line, context.left.Column), Visit(context.right), line, column);
        }

        public override IAST VisitDivAssignStatement(llParser.DivAssignStatementContext context)
        {
            string variableName = context.left.Text;
            int line = context.Start.Line;
            int column = context.Start.Column;

            if (!this.Env.ContainsKey(variableName))
                throw new UnknownVariableException(variableName, this.RootProgram.FileName, line, column);

            return new DivAssignStatement(new VarExpr(variableName, this.TryGetType(variableName, line, column), context.left.Line, context.left.Column), Visit(context.right), line, column);
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

        public override IAST VisitCharArrayCreation([NotNull] llParser.CharArrayCreationContext context)
        {
            return new CharArray(Visit(context.expression()), context.Start.Line, context.Start.Column);
        }

        public override IAST VisitRefTypeCreation(llParser.RefTypeCreationContext context)
        {
            int line = context.Start.Line;
            int column = context.Start.Column;

            if (context.arrayCreation() != null)
                return new RefTypeCreationStatement(Visit(context.arrayCreation()), context.Start.Line, context.Start.Column);
            if (context.structCreation() != null)
                return new RefTypeCreationStatement(Visit(context.structCreation()), context.Start.Line, context.Start.Column);

            throw new UnknownTypeException(context.GetText(), this.RootProgram.FileName, line, column);
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

        public override IAST VisitGlobalVariableStatement([NotNull] llParser.GlobalVariableStatementContext context)
        {
            string varName = context.name.Text;
            bool success = this.RootProgram.GlobalVariables.TryGetValue(varName, out GlobalVariableStatement globalVariable);
            int line = context.Start.Line;
            int column = context.Start.Column;

            if(!success)
                throw new UnknownVariableException(varName, this.RootProgram.FileName, line, column);

            IAST val = null;
            if(context.CHAR_LITERAL() != null)
                val = this.Visit(context.CHAR_LITERAL());

            if(context.numericExpression() != null)
                val = this.Visit(context.numericExpression());
            
            if(context.boolExpression() != null)
                val = this.Visit(context.boolExpression());
            
            if(context.refTypeCreation() != null)
                val = this.Visit(context.refTypeCreation());

            if(val is null)
                throw new NoValueException(varName, this.RootProgram.FileName, line, column);
            
            globalVariable.Value = val;

            return globalVariable;
        }

        private LL.Types.Type TryGetType(string varName, int line, int column)
        {
            bool success = this.Env.TryGetValue(varName, out IAST @var);

            if (!success)
                throw new UnknownVariableException(varName, null, line, column);

            return @var.Type;
        }
    }
}