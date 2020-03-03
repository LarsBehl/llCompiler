using System;
using System.Globalization;
using System.Collections.Generic;
using ll.AST;
using ll.type;

namespace ll
{
    public class BuildAstVisitor : llBaseVisitor<IAST>
    {
        static VarExpr sR = null;
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

            throw new ArgumentException($"Unknown AST Node; On line {context.Start.Line}:{context.Start.Column}");
        }

        public override IAST VisitCompositUnit(llParser.CompositUnitContext context)
        {
            if (context.statement() != null)
                return Visit(context.statement());

            if (context.expression() != null)
                return Visit(context.expression());

            throw new ArgumentException($"Unknown node; On line {context.Start.Line}:{context.Start.Column}");
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
            return new IntLit(Int32.Parse(sign + context.INTEGER_LITERAL().GetText()), context.Start.Line, context.Start.Column);
        }

        public override IAST VisitDoubleAtomExpression(llParser.DoubleAtomExpressionContext context)
        {
            string sign = "+";
            if (context.sign != null)
                sign = context.sign.Text;
            return new DoubleLit(Double.Parse(sign + context.DOUBLE_LITERAL().GetText(), new CultureInfo("en-US").NumberFormat), context.Start.Line, context.Start.Column);
        }

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

        public override IAST VisitVariableExpression(llParser.VariableExpressionContext context)
        {
            type.Type type = null;

            if(sR != null)
            {
                try
                {
                    // search in the current struct for the accessed property and the assosiated type
                    type = IAST.structs[(sR.type as StructType).structName].properties.Find(s => s.name == context.WORD().GetText()).type;
                }
                catch
                {
                    throw new ArgumentException($"Unknown struct property {context.WORD().GetText()}; On line {context.Start.Line}:{context.Start.Column}");
                }

                return new VarExpr(context.WORD().GetText(), type, context.Start.Line, context.Start.Column);
            }
            
            return new VarExpr(context.WORD().GetText(), context.Start.Line, context.Start.Column);
        }

        public override IAST VisitAssignStatement(llParser.AssignStatementContext context)
        {
            IAST right;

            if (!IAST.env.ContainsKey(context.left.Text))
                throw new ArgumentException($"Unknown variable \"{context.left.Text}\"; On line {context.left.Line}:{context.left.Column}");

            // check if righthand side of the assignment is an array or an expression
            if (context.expression() != null)
                right = Visit(context.expression());
            else
                right = Visit(context.refTypeCreation());

            return new AssignStatement(new VarExpr(context.left.Text, context.Start.Line, context.left.StartIndex), right, context.Start.Line, context.Start.Column);
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

            return new BlockStatement(body, context.Start.Line, context.Start.Column);
        }

        public override IAST VisitReturnStatement(llParser.ReturnStatementContext context)
        {
            if (context.expression() != null)
                return new ReturnStatement(Visit(context.expression()), context.Start.Line, context.Start.Column);
            if (context.refTypeCreation() != null)
                return new ReturnStatement(Visit(context.refTypeCreation()), context.Start.Line, context.Start.Column);

            return new ReturnStatement(context.Start.Line, context.Start.Column);
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

        public override IAST VisitInitializationStatement(llParser.InitializationStatementContext context)
        {
            IAST variable = Visit(context.type);

            if (variable.type is VoidType)
                throw new ArgumentException($"Type \"{variable.type.typeName}\" not allowed for variables; On line {variable.line}:{variable.column}");

            IAST.env[context.left.Text] = variable;

            IAST val;
            if (context.expression() != null)
                val = Visit(context.expression());
            else
                val = Visit(context.refTypeCreation());

            if (variable.type.typeName != val.type.typeName)
            {
                if (variable.type is DoubleType && val.type is IntType
                || variable.type is RefType && val.type is RefType)
                    return new AssignStatement(new VarExpr(context.left.Text, context.Start.Line, context.left.StartIndex), val, context.Start.Line, context.Start.Column);
                else
                    throw new ArgumentException($"Type \"{val.type.typeName}\" does not match \"{variable.type.typeName}\"; On line {context.Start.Line}:{context.Start.Column}");
            }

            return new AssignStatement(new VarExpr(context.left.Text, context.Start.Line, context.left.StartIndex), val, context.Start.Line, context.Start.Column);
        }

        public override IAST VisitTypeDefinition(llParser.TypeDefinitionContext context)
        {
            if (context.INT_TYPE() != null)
                return new IntLit(null, context.Start.Line, context.Start.Column);
            if (context.DOUBLE_TYPE() != null)
                return new DoubleLit(null, context.Start.Line, context.Start.Column);
            if (context.BOOL_TYPE() != null)
                return new BoolLit(null, context.Start.Line, context.Start.Column);
            if (context.VOID_TYPE() != null)
                return new VoidLit(context.Start.Line, context.Start.Column);
            if (context.arrayTypes() != null)
                return Visit(context.arrayTypes());
            if (context.structName() != null)
                return Visit(context.structName());
            throw new ArgumentException($"Unsupported type; On line {context.Start.Line}:{context.Start.Column}");
        }

        public override IAST VisitBoolExpression(llParser.BoolExpressionContext context)
        {
            if (context.BOOL_FALSE() != null)
                return new BoolLit(false, context.Start.Line, context.Start.Column);
            if (context.BOOL_TRUE() != null)
                return new BoolLit(true, context.Start.Line, context.Start.Column);

            throw new ArgumentException($"Unsupportet value for bool; On line {context.Start.Line}:{context.Start.Column}");
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

        public override IAST VisitInstantiationStatement(llParser.InstantiationStatementContext context)
        {
            IAST variable = Visit(context.type);

            if (variable.type is VoidType)
                throw new ArgumentException($"Type \"{variable.type.typeName}\" is not allowed for variables; On line {variable.line}:{variable.column}");

            IAST.env[context.left.Text] = variable;

            return new InstantiationStatement(context.WORD().GetText(), variable.type, context.Start.Line, context.Start.Column);
        }

        public override IAST VisitFunctionDefinition(llParser.FunctionDefinitionContext context)
        {
            var identifier = context.WORD();
            FunctionDefinition funDef = IAST.funs[identifier[0].GetText()];
            IAST.env = funDef.functionEnv;
            // save the new function definition
            if (funDef.body != null)
                throw new ArgumentException($"Trying to override the body of function \"{identifier[0].GetText()}\"; On line {context.Start.Line}:{context.Start.Column}");
            var body = Visit(context.body) as BlockStatement;

            if ((body.type.typeName != funDef.returnType.typeName || !body.doesFullyReturn)
            && !(funDef.returnType is VoidType)
            && !(body.type is RefType && funDef.returnType is RefType))
            {
                if (body.type is BlockStatementType || !body.doesFullyReturn)
                    throw new ArgumentException($"Missing return statement in \"{funDef.name}\"; On line {context.Start.Line}:{context.Start.Column}");
                throw new ArgumentException($"Return type \"{body.type.typeName}\" does not match \"{funDef.returnType.typeName}\"; On line {context.Start.Line}:{context.Start.Column}");
            }

            if ((funDef.returnType is VoidType) && (!(body.type is VoidType) && !(body.type is BlockStatementType)))
                throw new ArgumentException($"Could not return \"{body.type.typeName}\" in a void function; On line {context.Start.Line}:{context.Start.Column}");

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

            return new FunctionCall(context.name.Text, args, context.Start.Line, context.Start.Column);
        }

        public override IAST VisitProgram(llParser.ProgramContext context)
        {
            List<IAST> funDefs = new List<IAST>();
            List<IAST> structDefs = new List<IAST>();
            var structs = context.structDefinition();
            var funs = context.functionDefinition();

            if (funs?.Length > 0 || structs?.Length > 0)
            {
                foreach (var structDef in context.structDefinition())
                    structDefs.Add(Visit(structDef));

                foreach (var funDef in funs)
                    funDefs.Add(Visit(funDef));

                return new ProgramNode(funDefs, structDefs, context.Start.Line, context.Start.Column);
            }

            if (context.compositUnit() != null)
                return Visit(context.compositUnit());

            throw new ArgumentException($"Unknown node in Program; On line {context.Start.Line}:{context.Start.Column}");
        }

        public override IAST VisitIfStatement(llParser.IfStatementContext context)
        {
            var tmp = context.blockStatement();
            var cond = Visit(context.cond);
            var ifBody = Visit(tmp[0]);
            IAST elseBody = null;

            if (tmp.Length > 1)
                elseBody = Visit(tmp[1]);

            return new IfStatement(cond, ifBody, elseBody, context.Start.Line, context.Start.Column);
        }

        public override IAST VisitWhileStatement(llParser.WhileStatementContext context)
        {
            var cond = Visit(context.cond);
            var body = Visit(context.blockStatement());

            return new WhileStatement(cond, body, context.Start.Line, context.Start.Column);
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

        public override IAST VisitAddAssignStatement(llParser.AddAssignStatementContext context)
        {
            if (!IAST.env.ContainsKey(context.left.Text))
                throw new ArgumentException($"Unknown variable \"{context.left.Text}\"; On line {context.Start.Line}:{context.Start.Column}");

            return new AddAssignStatement(new VarExpr(context.left.Text, context.left.Line, context.left.Column), Visit(context.right), context.Start.Line, context.Start.Column);
        }

        public override IAST VisitSubAssignStatement(llParser.SubAssignStatementContext context)
        {
            if (!IAST.env.ContainsKey(context.left.Text))
                throw new ArgumentException($"Unknown variable \"{context.left.Text}\"; On line {context.Start.Line}:{context.Start.Column}");

            return new SubAssignStatement(new VarExpr(context.left.Text, context.left.Line, context.left.Column), Visit(context.right), context.Start.Line, context.Start.Column);
        }

        public override IAST VisitMultAssignStatement(llParser.MultAssignStatementContext context)
        {
            if (!IAST.env.ContainsKey(context.left.Text))
                throw new ArgumentException($"Unknown variable {context.left.Text}; On line {context.Start.Line}:{context.Start.Column}");

            return new MultAssignStatement(new VarExpr(context.left.Text, context.left.Line, context.left.Column), Visit(context.right), context.Start.Line, context.Start.Column);
        }

        public override IAST VisitDivAssignStatement(llParser.DivAssignStatementContext context)
        {
            if (!IAST.env.ContainsKey(context.left.Text))
                throw new ArgumentException($"Unknown variable {context.left.Text}; On line {context.Start.Line}:{context.Start.Column}");

            return new DivAssignStatement(new VarExpr(context.left.Text, context.left.Line, context.left.Column), Visit(context.right), context.Start.Line, context.Start.Column);
        }

        public override IAST VisitNotExpression(llParser.NotExpressionContext context)
        {
            return new NotExpr(Visit(context.expression()), context.Start.Line, context.Start.Column);
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

        public override IAST VisitPrintStatement(llParser.PrintStatementContext context)
        {
            return new PrintStatement(Visit(context.expression()), context.Start.Line, context.Start.Column);
        }

        // TODO rework arrays so it is possible to create arrays of reference types
        public override IAST VisitIntArrayType(llParser.IntArrayTypeContext context)
        {
            return new IntArray(context.Start.Line, context.Start.Column);
        }

        public override IAST VisitDoubleArrayType(llParser.DoubleArrayTypeContext context)
        {
            return new DoubleArray(context.Start.Line, context.Start.Column);
        }

        public override IAST VisitBoolArrayType(llParser.BoolArrayTypeContext context)
        {
            return new BoolArray(context.Start.Line, context.Start.Column);
        }

        public override IAST VisitIntArrayCreation(llParser.IntArrayCreationContext context)
        {
            return new IntArray(Visit(context.expression()), context.Start.Line, context.Start.Column);
        }

        public override IAST VisitDoubleArrayCreation(llParser.DoubleArrayCreationContext context)
        {
            return new DoubleArray(Visit(context.expression()), context.Start.Line, context.Start.Column);
        }

        public override IAST VisitBoolArrayCreation(llParser.BoolArrayCreationContext context)
        {
            return new BoolArray(Visit(context.expression()), context.Start.Line, context.Start.Column);
        }

        public override IAST VisitRefTypeCreation(llParser.RefTypeCreationContext context)
        {
            if (context.arrayCreation() != null)
                return new RefTypeCreationStatement(Visit(context.arrayCreation()), context.Start.Line, context.Start.Column);
            if (context.structCreation() != null)
                return new RefTypeCreationStatement(Visit(context.structCreation()), context.Start.Line, context.Start.Column);

            throw new ArgumentException($"Invalid type for reference type creation; On line {context.Start.Line}:{context.Start.Column}");
        }

        public override IAST VisitArrayIndexing(llParser.ArrayIndexingContext context)
        {
            return new ArrayIndexing(Visit(context.variableExpression()), Visit(context.expression()), context.Start.Line, context.Start.Column);
        }

        public override IAST VisitAssignArrayField(llParser.AssignArrayFieldContext context)
        {
            ArrayIndexing arrayIndexing = Visit(context.arrayIndexing()) as ArrayIndexing;

            return new AssignArrayField(arrayIndexing, Visit(context.expression()), context.Start.Line, context.Start.Column);
        }

        public override IAST VisitDestructionStatement(llParser.DestructionStatementContext context)
        {
            return Visit(context.refTypeDestruction());
        }

        public override IAST VisitRefTypeDestruction(llParser.RefTypeDestructionContext context)
        {
            return new DestructionStatement(Visit(context.variableExpression()) as VarExpr, context.Start.Line, context.Start.Column);
        }

        public override IAST VisitStructDefinition(llParser.StructDefinitionContext context)
        {
            string name = context.WORD().GetText();

            if (!IAST.structs.ContainsKey(name))
                throw new ArgumentException($"Unknown struct \"{name}\"; On line {context.WORD().Symbol.Line}:{context.WORD().Symbol.Column}");

            StructDefinition structDef = IAST.structs[name];

            if (structDef.properties != null)
                throw new ArgumentException($"Multiple definitions of struct \"{name}\"; On line {context.WORD().Symbol.Line}:{context.WORD().Symbol.Column}");

            var props = context.structProperties();
            List<StructProperty> properties = new List<StructProperty>();

            foreach (var prop in props)
            {
                var tmp = Visit(prop) as StructProperty;

                if (properties.FindIndex(s => s.name == tmp.name) >= 0)
                    throw new ArgumentException($"Multiple definitions of \"{tmp.name}\" in struct \"{name}\"; On line {context.Start.Line}:{context.Start.Column}");

                properties.Add(tmp);
            }

            structDef.properties = properties;

            return structDef;
        }

        public override IAST VisitStructName(llParser.StructNameContext context)
        {
            string name = context.WORD().GetText();

            if (!IAST.structs.ContainsKey(name))
                throw new ArgumentException($"Unknown struct \"{name}\"; On line {context.Start.Line}:{context.Start.Column}");

            return new Struct(name, context.Start.Line, context.Start.Column);
        }

        public override IAST VisitStructProperties(llParser.StructPropertiesContext context)
        {
            return new StructProperty(context.WORD().GetText(), Visit(context.typeDefinition()).type, context.Start.Line, context.Start.Column);
        }

        public override IAST VisitStructCreation(llParser.StructCreationContext context)
        {
            return Visit(context.structName());
        }

        /*
        * TODO change the grammer so that the struct prop access could have:
        *   - struct prop access done
        *   - array indexing done
        *   - variable expression done
        *   - increment done
        *   - decrement done
        *   - array assign
        *   - struct prop assign
        * on the righthand site
        */
        public override IAST VisitStructPropertyAccess(llParser.StructPropertyAccessContext context)
        {
            VarExpr v = Visit(context.variableExpression()) as VarExpr;
            sR = v;
            ValueAccessExpression right = Visit(context.valueAccess()) as ValueAccessExpression;
            sR = null;
            return new StructPropertyAccess(v, right, context.Start.Line, context.Start.Column);
        }

        public override IAST VisitAssignStructProp(llParser.AssignStructPropContext context)
        {
            StructPropertyAccess structPropAccess = Visit(context.structPropertyAccess()) as StructPropertyAccess;
            IAST val;

            if (context.expression() != null)
                val = Visit(context.expression());
            else
                val = Visit(context.refTypeCreation());

            return new AssignStructProperty(structPropAccess, val, context.Start.Line, context.Start.Column);
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
    }
}