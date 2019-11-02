using System;
using System.Globalization;
using System.Collections.Generic;

namespace ll
{
    public class BuildAstVisitor : llBaseVisitor<IAST>
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

        public override IAST VisitBinOpAddSub(llParser.BinOpAddSubContext context)
        {
            switch(context.op.Text)
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

        public override IAST VisitAssignExpression(llParser.AssignExpressionContext context)
        {
            return new AssignExpr(new VarExpr(context.left.Text), Visit(context.right));
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

            body.Add(Visit(context.returnExpression()));

            return new ExpressionSequenz(body);
        }

        public override IAST VisitReturnExpression(llParser.ReturnExpressionContext context)
        {
            return new ReturnExpr(Visit(context.expression()));
        }
    }
}