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
            return Visit(context.compositUnit());
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
            return new AssignStatement(new VarExpr(context.left.Text), Visit(context.right));
        }

        public override IAST VisitBlockSta(llParser.BlockStaContext context)
        {
            return Visit(context.blockStatement());
        }

        public override IAST VisitBlockStatement(llParser.BlockStatementContext context)
        {
            List<IAST> body = new List<IAST>();
            var tmp = context.compositUnit();

            foreach(var comp in tmp)
            {
                var compVisited = Visit(comp);
                Console.WriteLine(compVisited.type.typeName);
                body.Add(compVisited);
                if(compVisited is ReturnStatement)
                    break;
            }

            return new BlockStatement(body);
        }

        public override IAST VisitReturnStatement(llParser.ReturnStatementContext context)
        {
            return new ReturnStatement(Visit(context.expression()));
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

            IAST.env[context.left.Text] = variable;

            IAST val = Visit(context.right);
            if (variable.type.typeName != val.type.typeName)
                throw new ArgumentException($"Type \"{val.type.typeName}\" does not match \"{variable.type.typeName}\"");

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

            throw new ArgumentException("Unknown unary type");
        }

        public override IAST VisitInstantiationStatement(llParser.InstantiationStatementContext context)
        {
            IAST variable = Visit(context.type);

            IAST.env[context.left.Text] = variable;

            return new InstantiationStatement(context.WORD().GetText(), variable.type);
        }

        public override IAST VisitFunctionDefinition(llParser.FunctionDefinitionContext context)
        {
            var identifier = context.WORD();
            var types = context.typeDefinition();
            List<InstantiationStatement> args = new List<InstantiationStatement>();
            var tmpEnv = new Dictionary<string, IAST>();
            IAST.env = tmpEnv;

            // initialize the environment and arguments for this function definition
            for (int i = 0; i < types.Length - 1; i++)
            {
                var tmpType = Visit(types[i]);
                tmpEnv[identifier[i + 1].GetText()] = tmpType;
                args.Add(new InstantiationStatement(identifier[i + 1].GetText(), tmpType.type));
            }
            
            // create the resulting object
            FunctionDefinition func = new FunctionDefinition(identifier[0].GetText(), args, Visit(context.blockStatement()), tmpEnv, Visit(types[types.Length - 1]).type);
            // save the new function definition
            IAST.funs[identifier[0].GetText()] = func;

            return func;
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
    }
}