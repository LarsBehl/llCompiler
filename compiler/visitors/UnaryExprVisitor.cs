using System;
using System.Collections.Generic;
using System.Globalization;
using LL.AST;
using LL.Types;
using LL.Exceptions;

namespace LL
{
    public partial class BuildAstVisitor : llBaseVisitor<IAST>
    {
        public override IAST VisitUnaryExpression(llParser.UnaryExpressionContext context)
        {
            int line = context.Start.Line;
            int column = context.Start.Column;

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
                return new NullLit(line, column);
            if (context.structPropertyAccess() != null)
                return Visit(context.structPropertyAccess());

            throw new NodeNotImplementedException(context.GetText(), this.CurrentFile, line, column);
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
            int line = context.Start.Line;
            int column = context.Start.Column;

            if (context.BOOL_FALSE() != null)
                return new BoolLit(false, line, column);
            if (context.BOOL_TRUE() != null)
                return new BoolLit(true, line, column);

            throw new NodeNotImplementedException(context.GetText(), this.CurrentFile, line, column);
        }

        public override IAST VisitFunctionCall(llParser.FunctionCallContext context)
        {
            string name = context.name.Text;
            int line = context.Start.Line;
            int column = context.Start.Column;
            var tmp = context.expression();
            if (!this.RootProgram.FunDefs.ContainsKey(context.name.Text))
                throw new UnknownFunctionException(name, this.CurrentFile, line, column);
            List<IAST> args = new List<IAST>();

            foreach (var arg in tmp)
            {
                args.Add(Visit(arg));
            }

            return new FunctionCall(this.RootProgram.FunDefs[name], name, args, line, column);
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

            throw new NodeNotImplementedException(context.GetText(), this.CurrentFile, context.Start.Line, context.Start.Column);
        }

        public override IAST VisitNotExpression(llParser.NotExpressionContext context)
        {
            return new NotExpr(Visit(context.expression()), context.Start.Line, context.Start.Column);
        }

        public override IAST VisitVariableExpression(llParser.VariableExpressionContext context)
        {
            Types.Type type = null;
            int line = context.Start.Line;
            int column = context.Start.Column;
            string variableName = context.WORD().GetText();

            if (sR != null)
            {
                string structName = (sR.Type as StructType).StructName;

                // search for the struct definition
                bool success = this.RootProgram.StructDefs.TryGetValue(structName, out StructDefinition def);
                if (!success)
                    throw new UnknownTypeException(structName, this.CurrentFile, line, column);

                // search for the property in the struct
                IAST prop = def.Properties.Find(s => s.Name == variableName);
                if (prop is null)
                    throw new UnknownVariableException($"{structName}.{variableName}", this.CurrentFile, line, column);

                type = prop.Type;

                return new VarExpr(variableName, type, line, column);
            }

            return new VarExpr(variableName, this.TryGetType(variableName, line, column), line, column);
        }
    }
}