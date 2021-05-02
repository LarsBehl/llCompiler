using System;
using System.Collections.Generic;
using System.Globalization;
using LL.AST;
using LL.Types;

namespace LL
{
    public partial class BuildAstVisitor : llBaseVisitor<IAST>
    {
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
            if (context.notExpression() != null)
                return Visit(context.notExpression());
            if (context.arrayIndexing() != null)
                return Visit(context.arrayIndexing());
            if (context.NULL() != null)
                return new NullLit(context.Start.Line, context.Start.Column);
            if (context.structPropertyAccess() != null)
                return Visit(context.structPropertyAccess());

            throw new ArgumentException($"Unknown unary type; On line {context.Start.Line}:{context.Start.Column}");
        }

        public override IAST VisitIntegerAtomExpression(llParser.IntegerAtomExpressionContext context)
        {
            string sign = "+";
            if (context.sign != null)
                sign = context.sign.Text;
            return new IntLit(Int32.Parse(sign + context.INTEGER_LITERAL().GetText()), context.Start.Line, context.Start.Column);
        }

        public override IAST VisitDoubleAtomExpression(llParser.DoubleAtomExpressionContext context)
        {
            string sign = "+";
            if (context.sign != null)
                sign = context.sign.Text;
            return new DoubleLit(Double.Parse(sign + context.DOUBLE_LITERAL().GetText(), new CultureInfo("en-US").NumberFormat), context.Start.Line, context.Start.Column);
        }

        public override IAST VisitBoolExpression(llParser.BoolExpressionContext context)
        {
            if (context.BOOL_FALSE() != null)
                return new BoolLit(false, context.Start.Line, context.Start.Column);
            if (context.BOOL_TRUE() != null)
                return new BoolLit(true, context.Start.Line, context.Start.Column);

            throw new ArgumentException($"Unsupportet value for bool; On line {context.Start.Line}:{context.Start.Column}");
        }

        public override IAST VisitFunctionCall(llParser.FunctionCallContext context)
        {
            var tmp = context.expression();
            List<IAST> args = new List<IAST>();

            foreach (var arg in tmp)
            {
                args.Add(Visit(arg));
            }

            return new FunctionCall(context.name.Text, args, context.Start.Line, context.Start.Column);
        }

        public override IAST VisitIncrementPostExpression(llParser.IncrementPostExpressionContext context)
        {
            ValueAccessExpression variable = Visit(context.valueAccess()) as ValueAccessExpression;

            return new IncrementExpr(variable, true, context.Start.Line, context.Start.Column);
        }

        public override IAST VisitDecrementPostExpression(llParser.DecrementPostExpressionContext context)
        {
            ValueAccessExpression variable = Visit(context.valueAccess()) as ValueAccessExpression;

            return new DecrementExpr(variable, true, context.Start.Line, context.Start.Column);
        }

        public override IAST VisitIncrementPreExpression(llParser.IncrementPreExpressionContext context)
        {
            ValueAccessExpression variable = Visit(context.valueAccess()) as ValueAccessExpression;

            return new IncrementExpr(variable, false, context.Start.Line, context.Start.Column);
        }

        public override IAST VisitDecrementPreExpression(llParser.DecrementPreExpressionContext context)
        {
            ValueAccessExpression variable = Visit(context.valueAccess()) as ValueAccessExpression;

            return new DecrementExpr(variable, false, context.Start.Line, context.Start.Column);
        }

        public override IAST VisitArrayIndexing(llParser.ArrayIndexingContext context)
        {
            var tmp = sR;
            sR = null;
            var index = Visit(context.expression());
            sR = tmp;

            return new ArrayIndexing(Visit(context.variableExpression()), index, context.Start.Line, context.Start.Column);
        }

        public override IAST VisitStructPropertyAccess(llParser.StructPropertyAccessContext context)
        {
            VarExpr v = Visit(context.variableExpression()) as VarExpr;
            sR = v;
            ValueAccessExpression right = Visit(context.valueAccess()) as ValueAccessExpression;
            sR = null;
            return new StructPropertyAccess(v, right, context.Start.Line, context.Start.Column);
        }

        public override IAST VisitValueAccess(llParser.ValueAccessContext context)
        {
            if (context.arrayIndexing() != null)
                return Visit(context.arrayIndexing());

            if (context.variableExpression() != null)
                return Visit(context.variableExpression());

            if (context.structPropertyAccess() != null)
                return Visit(context.structPropertyAccess());

            throw new ArgumentException($"Unknown value access; On line {context.Start.Line}:{context.Start.Column}");
        }

        public override IAST VisitNotExpression(llParser.NotExpressionContext context)
        {
            return new NotExpr(Visit(context.expression()), context.Start.Line, context.Start.Column);
        }

        public override IAST VisitVariableExpression(llParser.VariableExpressionContext context)
        {
            Types.Type type = null;

            if (sR != null)
            {
                try
                {
                    // search in the current struct for the accessed property and the assosiated type
                    type = IAST.Structs[(sR.Type as StructType).structName].Properties.Find(s => s.Name == context.WORD().GetText()).Type;
                }
                catch
                {
                    throw new ArgumentException($"Unknown struct property {context.WORD().GetText()}; On line {context.Start.Line}:{context.Start.Column}");
                }

                return new VarExpr(context.WORD().GetText(), type, context.Start.Line, context.Start.Column);
            }

            return new VarExpr(context.WORD().GetText(), context.Start.Line, context.Start.Column);
        }
    }
}