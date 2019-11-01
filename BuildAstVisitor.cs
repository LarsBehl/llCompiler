using System;
using System.Globalization;
using System.Collections.Generic;

namespace ll
{
    class BuildAstVisitor : llBaseVisitor<IAST>
    {
        public override IAST VisitCompileUnit(llParser.CompileUnitContext context)
        {
            return Visit(context.expression());
        }

        public override IAST VisitParenthes(llParser.ParenthesContext context)
        {
            return Visit(context.expression());
        }

        public override IAST VisitNumericAtomExpression(llParser.NumericAtomExpressionContext context)
        {
            return Visit(context.numericExpression());
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

        public override IAST VisitVarExpr(llParser.VarExprContext context)
        {
            return Visit(context.variableExpression());
        }

        // TODO rewrite that it gets added to environment on evaluation
        public override IAST VisitVariableExpression(llParser.VariableExpressionContext context)
        {
            IAST.environment.TryAdd(context.WORD().GetText(), 0);

            return new VarExpr(context.WORD().GetText());

        }

        public override IAST VisitAssignExpression(llParser.AssignExpressionContext context)
        {
            return new AssignExpr(new VarExpr(context.left.Text), Visit(context.right));
        }

        public override IAST VisitInfixExpression(llParser.InfixExpressionContext context)
        {
            switch (context.op.Text)
            {
                case "+": return new AddExpr(Visit(context.left), Visit(context.right));
                case "-": return new SubExpr(Visit(context.left), Visit(context.right));
                case "*": return new MultExpr(Visit(context.left), Visit(context.right));
                case "/": return new DivExpr(Visit(context.left), Visit(context.right));
                default:
                    Console.WriteLine("Unknown op {0}", context.op.Text);
                    return null;
            }
        }

        public override IAST VisitExprSequ(llParser.ExprSequContext context)
        {
            return Visit(context.expressionSequenz());
        }

        public override IAST VisitExpressionSequenz(llParser.ExpressionSequenzContext context)
        {
            List<IAST> body = new List<IAST>();
            var tmp = context.expression();
            for(int i = 0; i < tmp.Length; i++)
            {
                body.Add(Visit(tmp[i]));
            }

            return new Sequenz(body);
        }
    }
}