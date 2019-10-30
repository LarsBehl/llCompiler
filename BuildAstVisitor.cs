using System;
using System.Globalization;

namespace ll
{
    class BuildAstVisitor : LParserBaseVisitor<IAST>
    {
        public override IAST VisitCompileUnit(LParserParser.CompileUnitContext context)
        {
            return Visit(context.expression());
        }

        public override IAST VisitParenthes(LParserParser.ParenthesContext context)
        {
            return Visit(context.expression());
        }

        public override IAST VisitNumericAtomExpression(LParserParser.NumericAtomExpressionContext context)
        {
            return Visit(context.numericExpression());
        }

        public override IAST VisitIntegerAtomExpression(LParserParser.IntegerAtomExpressionContext context)
        {
            string sign = "+";
            if (context.sign != null)
                sign = context.sign.Text;
            return new IntLit(Int32.Parse(sign + context.INTEGER_LITERAL().GetText()));
        }

        public override IAST VisitDoubleAtomExpression(LParserParser.DoubleAtomExpressionContext context)
        {
            string sign = "+";
            if (context.sign != null)
                sign = context.sign.Text;
            return new DoubleLit(Double.Parse(sign + context.DOUBLE_LITERAL().GetText(), new CultureInfo("en-US").NumberFormat));
        }

        // TODO rewrite that it gets added to environment on evaluation
        public override IAST VisitVariableExpression(LParserParser.VariableExpressionContext context)
        {
            IAST.environment.TryAdd(context.WORD().GetText(), 0);

            return new VarExpr(context.WORD().GetText());

        }

        public override IAST VisitAssignExpression(LParserParser.AssignExpressionContext context)
        {
            return new AssignExpr(new VarExpr(context.left.Text), Visit(context.right));
        }

        public override IAST VisitInfixExpression(LParserParser.InfixExpressionContext context)
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
    }
}