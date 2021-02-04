using System;
using ll.AST;

namespace ll
{
    public partial class BuildAstVisitor : llBaseVisitor<IAST>
    {
        public override IAST VisitBinOpAddSub(llParser.BinOpAddSubContext context)
        {
            switch (context.op.Text)
            {
                case "+": return new AddExpr(Visit(context.left), Visit(context.right), context.Start.Line, context.Start.Column);
                case "-": return new SubExpr(Visit(context.left), Visit(context.right), context.Start.Line, context.Start.Column);
                default:
                    throw new ArgumentException($"unknown operator {context.op.Text}; On line {context.op.Line}:{context.op.Column}");
            }
        }

        public override IAST VisitBinOpMultDiv(llParser.BinOpMultDivContext context)
        {
            switch (context.op.Text)
            {
                case "*": return new MultExpr(Visit(context.left), Visit(context.right), context.Start.Line, context.Start.Column);
                case "/": return new DivExpr(Visit(context.left), Visit(context.right), context.Start.Line, context.Start.Column);
                default:
                    throw new ArgumentException($"unknown operator {context.op.Text}; On line {context.Start.Line}:{context.Start.Column}");
            }
        }

        public override IAST VisitEqualityOpertor(llParser.EqualityOpertorContext context)
        {
            return new EqualityExpr(Visit(context.left), Visit(context.right), context.Start.Line, context.Start.Column);
        }

        public override IAST VisitLessOperator(llParser.LessOperatorContext context)
        {
            return new LessExpr(Visit(context.left), Visit(context.right), context.ASSIGN() != null, context.Start.Line, context.Start.Column);
        }

        public override IAST VisitGreaterOperator(llParser.GreaterOperatorContext context)
        {
            return new GreaterExpr(Visit(context.left), Visit(context.right), context.ASSIGN() != null, context.Start.Line, context.Start.Column);
        }

        public override IAST VisitAndOperator(llParser.AndOperatorContext context)
        {
            return new AndExpr(Visit(context.left), Visit(context.right), context.Start.Line, context.Start.Column);
        }

        public override IAST VisitOrOperator(llParser.OrOperatorContext context)
        {
            return new OrExpr(Visit(context.left), Visit(context.right), context.Start.Line, context.Start.Column);
        }

        public override IAST VisitNotEqualOperator(llParser.NotEqualOperatorContext context)
        {
            return new NotEqualExpr(Visit(context.left), Visit(context.right), context.Start.Line, context.Start.Column);
        }

        public override IAST VisitBinOpMod(llParser.BinOpModContext context)
        {
            return new ModExpr(Visit(context.left), Visit(context.right), context.Start.Line, context.Start.Column);
        }
    }
}