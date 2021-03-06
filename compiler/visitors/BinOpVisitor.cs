using System;
using LL.AST;
using LL.Exceptions;

namespace LL
{
    public partial class BuildAstVisitor : llBaseVisitor<IAST>
    {
        public override IAST VisitBinOpAddSub(llParser.BinOpAddSubContext context)
        {
            string op = context.op.Text;
            int line = context.Start.Line;
            int column = context.Start.Column;

            switch (op)
            {
                case "+": return new AddExpr(Visit(context.left), Visit(context.right), line, column);
                case "-": return new SubExpr(Visit(context.left), Visit(context.right), line, column);
                default:
                    throw new UnknownOperatorException(op, this.RootProgram.FileName, line, column);
            }
        }

        public override IAST VisitBinOpMultDiv(llParser.BinOpMultDivContext context)
        {
            string op = context.op.Text;
            int line = context.Start.Line;
            int column = context.Start.Column;

            switch (op)
            {
                case "*": return new MultExpr(Visit(context.left), Visit(context.right), line, column);
                case "/": return new DivExpr(Visit(context.left), Visit(context.right), line, column);
                default:
                    throw new UnknownOperatorException(op, this.RootProgram.FileName, line, column);
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