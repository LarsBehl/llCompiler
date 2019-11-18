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
            if(context.statement() != null)
                return Visit(context.statement());

            if(context.expression() != null)
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

        public override IAST VisitAssignStatement(llParser.AssignStatementContext context)
        {
            return new AssignStatement(new VarExpr(context.left.Text), Visit(context.right));
        }

        public override IAST VisitExprSequ(llParser.ExprSequContext context)
        {
            return Visit(context.expressionSequenz());
        }

        public override IAST VisitExpressionSequenz(llParser.ExpressionSequenzContext context)
        {
            List<IAST> body = new List<IAST>();
            var tmp = context.compositUnit();
            for(int i = 0; i < tmp.Length; i++)
            {
                body.Add(Visit(tmp[i]));
            }

            body.Add(Visit(context.returnStatement()));

            return new ExpressionSequenz(body);
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
            ll.type.Type type = Visit(context.type).type;

            IAST val = Visit(context.right);
            if(type.typeName != val.type.typeName)
                throw new ArgumentException($"Type {val.type.typeName} does not match {type.typeName}");
            
            IAST.SetType(context.left.Text, type);

            return new AssignStatement(new VarExpr(context.left.Text), val);
        }

        public override IAST VisitTypeDefinition(llParser.TypeDefinitionContext context)
        {
            if(context.INT_TYPE() != null)
                return new IntLit(0);
            if(context.DOUBLE_TYPE() != null)
                return new DoubleLit(0.0);
            if(context.BOOL_TYPE() != null)
                return new BoolLit(false);
            throw new ArgumentException("Unsupported type");
        }

        public override IAST VisitBoolExpression(llParser.BoolExpressionContext context)
        {
            if(context.BOOL_FALSE() != null)
                return new BoolLit(false);
            if(context.BOOL_TRUE() != null)
                return new BoolLit(true);

            throw new ArgumentException("Unsupportet value for bool");
        }

        public override IAST VisitUnaryExpression(llParser.UnaryExpressionContext context)
        {
            if(context.boolExpression() != null)
                return Visit(context.boolExpression());
            if(context.numericExpression() != null)
                return Visit(context.numericExpression());
            if(context.variableExpression() != null)
                return Visit(context.variableExpression());
            if(context.functionCall() != null)
                return Visit(context.functionCall());

            throw new ArgumentException("Unknown unary type");
        }

        public override IAST VisitInstantiationStatement(llParser.InstantiationStatementContext context)
        {
            ll.type.Type tmp = Visit(context.type).type;
            IAST.SetType(context.WORD().GetText(), tmp);

            return new InstantiationStatement(context.WORD().GetText(), tmp);
        }

        public override IAST VisitFunctionDefinition(llParser.FunctionDefinitionContext context)
        {
            var tmp = context.WORD();
            var tmp2 = context.typeDefinition();
            List<string> args = new List<string>();

            for(int i = 0; i < tmp2.Length - 1; i++)
            {
                args.Add(tmp[i + 1].GetText());
                IAST.SetType(tmp[i + 1].GetText(), Visit(tmp2[i]).type);
            }

            FunctionDefinition func = new FunctionDefinition(tmp[0].GetText(), args, Visit(context.expressionSequenz()), Visit(tmp2[tmp2.Length - 1]).type);
            IAST.funs[tmp[0].GetText()] = func;

            return func;
        }

        public override IAST VisitFunctionCall(llParser.FunctionCallContext context)
        {
            var tmp = context.expression();
            List<IAST> args = new List<IAST>();

            foreach(var arg in tmp)
            {
                args.Add(Visit(arg));
            }

            return new FunctionCall(context.name.Text, args);
        }
    }
}