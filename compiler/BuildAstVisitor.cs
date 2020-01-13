using System;
using System.Globalization;
using System.Collections.Generic;
using ll.AST;
using ll.type;

namespace ll
{
    public class BuildAstVisitor : llBaseVisitor<IAST>
    {
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

            throw new ArgumentException("Unknown AST Node");
        }

        public override IAST VisitCompositUnit(llParser.CompositUnitContext context)
        {
            if (context.statement() != null)
                return Visit(context.statement());

            if (context.expression() != null)
                return Visit(context.expression());

            throw new ArgumentException("Unknown node");
        }

        public override IAST VisitParenthes(llParser.ParenthesContext context)
        {
            return Visit(context.expression());
        }

        public override IAST VisitIntegerAtomExpression(llParser.IntegerAtomExpressionContext context)
        {
            string sign = "+";
            if (context.sign != null)
                sign = context.sign.Text;
            return new IntLit(Int32.Parse(sign + context.INTEGER_LITERAL().GetText()));
        }

        public override IAST VisitDoubleAtomExpression(llParser.DoubleAtomExpressionContext context)
        {
            string sign = "+";
            if (context.sign != null)
                sign = context.sign.Text;
            return new DoubleLit(Double.Parse(sign + context.DOUBLE_LITERAL().GetText(), new CultureInfo("en-US").NumberFormat));
        }

        public override IAST VisitBinOpAddSub(llParser.BinOpAddSubContext context)
        {
            switch (context.op.Text)
            {
                case "+": return new AddExpr(Visit(context.left), Visit(context.right));
                case "-": return new SubExpr(Visit(context.left), Visit(context.right));
                default:
                    throw new ArgumentException("unknown operator {0}", context.op.Text);
            }
        }

        public override IAST VisitBinOpMultDiv(llParser.BinOpMultDivContext context)
        {
            switch (context.op.Text)
            {
                case "*": return new MultExpr(Visit(context.left), Visit(context.right));
                case "/": return new DivExpr(Visit(context.left), Visit(context.right));
                default:
                    throw new ArgumentException("unknown operator {0}", context.op.Text);
            }
        }

        public override IAST VisitVariableExpression(llParser.VariableExpressionContext context)
        {
            return new VarExpr(context.WORD().GetText());
        }

        public override IAST VisitAssignStatement(llParser.AssignStatementContext context)
        {
            if (!IAST.env.ContainsKey(context.left.Text))
                throw new ArgumentException($"Unknown variable \"{context.left.Text}\"");
            return new AssignStatement(new VarExpr(context.left.Text), Visit(context.right));
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

            return new BlockStatement(body);
        }

        public override IAST VisitReturnStatement(llParser.ReturnStatementContext context)
        {
            if (context.expression() != null)
                return new ReturnStatement(Visit(context.expression()));

            return new ReturnStatement();
        }

        public override IAST VisitEqualityOpertor(llParser.EqualityOpertorContext context)
        {
            return new EqualityExpr(Visit(context.left), Visit(context.right));
        }

        public override IAST VisitLessOperator(llParser.LessOperatorContext context)
        {
            return new LessExpr(Visit(context.left), Visit(context.right), context.ASSIGN() != null);
        }

        public override IAST VisitGreaterOperator(llParser.GreaterOperatorContext context)
        {
            return new GreaterExpr(Visit(context.left), Visit(context.right), context.ASSIGN() != null);
        }

        public override IAST VisitInitializationStatement(llParser.InitializationStatementContext context)
        {
            IAST variable = Visit(context.type);

            if (variable.type is VoidType)
                throw new ArgumentException($"Type \"{variable.type.typeName}\" not allowed for variables");

            IAST.env[context.left.Text] = variable;

            IAST val = Visit(context.right);
            if (variable.type.typeName != val.type.typeName)
            {
                if (variable.type is DoubleType && val.type is IntType)
                    return new AssignStatement(new VarExpr(context.left.Text), val);
                else
                    throw new ArgumentException($"Type \"{val.type.typeName}\" does not match \"{variable.type.typeName}\"");
            }

            return new AssignStatement(new VarExpr(context.left.Text), val);
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
            throw new ArgumentException("Unsupported type");
        }

        public override IAST VisitBoolExpression(llParser.BoolExpressionContext context)
        {
            if (context.BOOL_FALSE() != null)
                return new BoolLit(false);
            if (context.BOOL_TRUE() != null)
                return new BoolLit(true);

            throw new ArgumentException("Unsupportet value for bool");
        }

        public override IAST VisitUnaryExpression(llParser.UnaryExpressionContext context)
        {
            if (context.boolExpression() != null)
                return Visit(context.boolExpression());
            if (context.numericExpression() != null)
                return Visit(context.numericExpression());
            if (context.variableExpression() != null)
                return Visit(context.variableExpression());
            if (context.functionCall() != null)
                return Visit(context.functionCall());
            if (context.incrementPostExpression() != null)
                return Visit(context.incrementPostExpression());
            if (context.decrementPostExpression() != null)
                return Visit(context.decrementPostExpression());
            if (context.decrementPreExpression() != null)
                return Visit(context.decrementPreExpression());
            if (context.incrementPreExpression() != null)
                return Visit(context.incrementPreExpression());

            throw new ArgumentException("Unknown unary type");
        }

        public override IAST VisitInstantiationStatement(llParser.InstantiationStatementContext context)
        {
            IAST variable = Visit(context.type);

            if (variable.type is VoidType)
                throw new ArgumentException($"Type \"{variable.type.typeName}\" is not allowed for variables");

            IAST.env[context.left.Text] = variable;

            return new InstantiationStatement(context.WORD().GetText(), variable.type);
        }

        public override IAST VisitFunctionDefinition(llParser.FunctionDefinitionContext context)
        {
            var identifier = context.WORD();
            FunctionDefinition funDef = IAST.funs[identifier[0].GetText()];
            IAST.env = funDef.functionEnv;
            // save the new function definition
            if (funDef.body != null)
                throw new ArgumentException($"Trying to override the body of function \"{identifier[0].GetText()}\"");
            var body = Visit(context.body);

            if (body.type.typeName != funDef.returnType.typeName)
            {
                if (body.type is BlockStatementType)
                    throw new ArgumentException($"Missing return statement in \"{funDef.name}\"");
                throw new ArgumentException($"Return type \"{body.type.typeName}\" does not match \"{funDef.returnType.typeName}\"");
            }

            funDef.body = body;
            IAST.funs[funDef.name] = funDef;

            return funDef;
        }

        public override IAST VisitFunctionCall(llParser.FunctionCallContext context)
        {
            var tmp = context.expression();
            List<IAST> args = new List<IAST>();

            foreach (var arg in tmp)
            {
                args.Add(Visit(arg));
            }

            return new FunctionCall(context.name.Text, args);
        }

        public override IAST VisitProgram(llParser.ProgramContext context)
        {
            List<IAST> funDefs = new List<IAST>();
            if (context.functionDefinition()?.Length > 0)
            {
                foreach (var funDef in context.functionDefinition())
                {
                    funDefs.Add(Visit(funDef));
                }

                return new ProgramNode(funDefs);
            }

            if (context.compositUnit() != null)
                return Visit(context.compositUnit());

            throw new ArgumentException("Unknown node in Program");
        }

        public override IAST VisitIfStatement(llParser.IfStatementContext context)
        {
            var tmp = context.blockStatement();
            var cond = Visit(context.cond);
            var ifBody = Visit(tmp[0]);
            IAST elseBody = null;

            if (tmp.Length > 1)
                elseBody = Visit(tmp[1]);

            return new IfStatement(cond, ifBody, elseBody);
        }

        public override IAST VisitWhileStatement(llParser.WhileStatementContext context)
        {
            var cond = Visit(context.cond);
            var body = Visit(context.blockStatement());

            return new WhileStatement(cond, body);
        }

        public override IAST VisitIncrementPostExpression(llParser.IncrementPostExpressionContext context)
        {
            var variable = Visit(context.variableExpression()) as VarExpr;

            return new IncrementExpr(variable, true);
        }

        public override IAST VisitDecrementPostExpression(llParser.DecrementPostExpressionContext context)
        {
            var variable = Visit(context.variableExpression()) as VarExpr;

            return new DecrementExpr(variable, true);
        }

        public override IAST VisitIncrementPreExpression(llParser.IncrementPreExpressionContext context)
        {
            var variable = Visit(context.variableExpression()) as VarExpr;

            return new IncrementExpr(variable, false);
        }

        public override IAST VisitDecrementPreExpression(llParser.DecrementPreExpressionContext context)
        {
            var variable = Visit(context.variableExpression()) as VarExpr;

            return new DecrementExpr(variable, false);
        }

        public override IAST VisitAddAssignStatement(llParser.AddAssignStatementContext context)
        {
            if (!IAST.env.ContainsKey(context.left.Text))
                throw new ArgumentException($"Unknown variable \"{context.left.Text}\"");

            return new AddAssignStatement(new VarExpr(context.left.Text), Visit(context.right));
        }

        public override IAST VisitSubAssignStatement(llParser.SubAssignStatementContext context)
        {
            if (!IAST.env.ContainsKey(context.left.Text))
                throw new ArgumentException($"Unknown variable \"{context.left.Text}\"");

            return new SubAssignStatement(new VarExpr(context.left.Text), Visit(context.right));
        }

        public override IAST VisitMultAssignStatement(llParser.MultAssignStatementContext context)
        {
            if (!IAST.env.ContainsKey(context.left.Text))
                throw new ArgumentException($"Unknown variable {context.left.Text}");

            return new MultAssignStatement(new VarExpr(context.left.Text), Visit(context.right));
        }

        public override IAST VisitDivAssignStatement(llParser.DivAssignStatementContext context)
        {
            if (!IAST.env.ContainsKey(context.left.Text))
                throw new ArgumentException($"Unknown variable {context.left.Text}");

            return new DivAssignStatement(new VarExpr(context.left.Text), Visit(context.right));
        }
    }
}