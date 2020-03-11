using System;
using System.Collections.Generic;
using ll.type;

namespace ll.AST
{
    public abstract class IAST
    {
        public static Dictionary<string, IAST> env;
        public static Dictionary<string, FunctionDefinition> funs = new Dictionary<string, FunctionDefinition>();
        public static Dictionary<string, StructDefinition> structs = new Dictionary<string, StructDefinition>();
        public static Dictionary<string, IAST> structEnv = null;
        public ll.type.Type type { get; set; }
        public int line { get; set; }
        public int column { get; set; }

        public IAST(ll.type.Type type, int line, int column)
        {
            this.type = type;
            this.line = line;
            this.column = column;
        }

        public IAST Eval()
        {
            switch (this)
            {
                case IntLit i: return i;
                case DoubleLit d: return d;
                case BoolLit b: return b;
                case MultExpr me:
                    return EvalMultExpression(me);
                case AddExpr add:
                    return EvalAddExpression(add);
                case SubExpr sub:
                    return EvalSubExpression(sub);
                case DivExpr div:
                    return EvalDivExpression(div);
                case VarExpr varExpr:
                    if (structEnv != null)
                        return structEnv[varExpr.name];
                    var envVar = env[varExpr.name];

                    switch (envVar)
                    {
                        case DoubleLit dl:
                            if (dl.n == null)
                                throw new ArgumentException($"Variable \"{varExpr.name}\" is not initialized");
                            break;
                        case IntLit il:
                            if (il.n == null)
                                throw new ArgumentException($"Variable \"{varExpr.name}\" is not initialized");
                            break;
                        case BoolLit bl:
                            if (bl.value == null)
                                throw new ArgumentException($"Variable \"{varExpr.name}\" is not initialized");
                            break;
                        case IntArray intArray:
                            if (((intArray.size.Eval() as IntLit).n ?? -1) < 0)
                                throw new ArgumentException($"Variable \"{varExpr.name}\" is not initialized");
                            break;
                        case DoubleArray doubleArray:
                            if (((doubleArray.size.Eval() as IntLit).n ?? -1) < 0)
                                throw new ArgumentException($"Variable \"{varExpr.name}\" is not initialized");
                            break;
                        case BoolArray boolArray:
                            if (((boolArray.size.Eval() as IntLit).n ?? -1) < 0)
                                throw new ArgumentException($"Variable \"{varExpr.name}\" is not initialized");
                            break;
                        default:
                            break;
                    }

                    return envVar.Eval();
                case AssignStatement assign:
                    var tmp = assign.value.Eval();
                    env[assign.variable.name] = tmp;
                    return null;
                case BlockStatement block:
                    IAST resultBlock = null;

                    foreach (var comp in block.body)
                    {
                        if (comp is ReturnStatement)
                        {
                            resultBlock = comp.Eval();
                            break;
                        }

                        if (comp is IfStatement)
                        {
                            resultBlock = comp.Eval();
                            if (resultBlock == null)
                                continue;
                            break;
                        }

                        if (comp is WhileStatement && !(comp.type is WhileStatementType))
                        {
                            resultBlock = comp.Eval();
                            break;
                        }

                        comp.Eval();
                    }

                    return resultBlock;
                case ReturnStatement returnExpr:
                    return returnExpr.returnValue?.Eval() ?? null;
                case EqualityExpr equalityExpr:
                    return EvalEqualityExpression(equalityExpr);
                case LessExpr lessExpr:
                    return EvalLessExpression(lessExpr);
                case GreaterExpr greaterExpr:
                    return EvalGreaterExpression(greaterExpr);
                case InstantiationStatement instantiation:
                    return null;
                case FunctionDefinition funDef:
                    return null;
                case FunctionCall funCall:
                    FunctionDefinition fDef = funs[funCall.name];
                    var newEnv = new Dictionary<string, IAST>(fDef.functionEnv);

                    for (int k = 0; k < funCall.args.Count; k++)
                    {
                        newEnv[fDef.args[k].name] = funCall.args[k].Eval();
                    }

                    var oldEnv = env;
                    env = newEnv;

                    var result = fDef.body.Eval();
                    env = oldEnv;

                    return result;
                case IfStatement ifStatement:
                    if ((ifStatement.cond.Eval() as BoolLit).value ?? false)
                        return ifStatement.ifBody.Eval();
                    return ifStatement.elseBody?.Eval() ?? null;
                case WhileStatement whileStatement:
                    IAST result_while = null;
                    while ((whileStatement.condition.Eval() as BoolLit).value ?? false)
                    {
                        result_while = whileStatement.body.Eval();
                        if (result_while != null && !(result_while.type is BlockStatementType))
                            break;
                    }

                    return result_while;
                case IncrementExpr increment:
                    return EvalIncrementExpr(increment);
                case DecrementExpr decrement:
                    return EvalDecrementExpr(decrement);
                case AddAssignStatement addAssign:
                    env[addAssign.left.name] = EvalAddAssign(addAssign);
                    return null;
                case SubAssignStatement subAssign:
                    env[subAssign.left.name] = EvalSubAssign(subAssign);
                    return null;
                case MultAssignStatement multAssign:
                    env[multAssign.left.name] = EvalMultAssign(multAssign);
                    return null;
                case DivAssignStatement divAssign:
                    env[divAssign.left.name] = EvalDivAssign(divAssign);
                    return null;
                case NotExpr notExpr:
                    return new BoolLit(!(notExpr.value.Eval() as BoolLit).value, notExpr.line, notExpr.column);
                case AndExpr andExpr:
                    return new BoolLit(
                        ((andExpr.left.Eval() as BoolLit).value ?? false)
                        && ((andExpr.right.Eval() as BoolLit).value ?? false), andExpr.line, andExpr.column);
                case OrExpr orExpr:
                    return new BoolLit(
                        ((orExpr.left.Eval() as BoolLit).value ?? false)
                        || ((orExpr.right.Eval() as BoolLit).value ?? false), orExpr.line, orExpr.column);
                case NotEqualExpr notEqualExpr:
                    return EvalNotEqualExpression(notEqualExpr);
                case ProgramNode programNode:
                    foreach (var f in programNode.funDefs)
                    {
                        f.Eval();
                    }
                    return null;
                case PrintStatement printStatement:
                    EvalPrintStatement(printStatement);
                    return null;
                case IntArray intArray:
                    return EvalIntArray(intArray);
                case DoubleArray doubleArray:
                    return EvalDoubleArray(doubleArray);
                case BoolArray boolArray:
                    return EvalBoolArray(boolArray);
                case RefTypeCreationStatement refType:
                    return EvalRefTypeCreationStatement(refType);
                case ArrayIndexing arrayIndexing:
                    return EvalArrayIndexing(arrayIndexing);
                case AssignArrayField assignArrayField:
                    EvalAssignArrayField(assignArrayField);
                    return null;
                case DestructionStatement destructionStatement:
                    throw new ArgumentException("Destruction Statement is not supported in interactive mode");
                case NullLit nullLit:
                    return nullLit;
                case StructDefinition structDef:
                    return null;
                case Struct @struct:
                    return @struct;
                case StructPropertyAccess structPropertyAccess:
                    return EvalStructPropertyAccess(structPropertyAccess);
                case AssignStructProperty assignStruct:
                    EvalAssignStructProperty(assignStruct);
                    return null;
                default:
                    throw new ArgumentException("Unknown Ast Object");
            }
        }

        static IAST EvalMultExpression(MultExpr me)
        {
            switch (me.left.type)
            {
                case IntType i:
                    if (me.right.type is IntType)
                        return new IntLit((me.left.Eval() as IntLit).n * (me.right.Eval() as IntLit).n, me.line, me.column);
                    if (me.right.type is DoubleType)
                        return new DoubleLit((me.left.Eval() as IntLit).n * (me.right.Eval() as DoubleLit).n, me.line, me.column);
                    throw new ArgumentException($"Type \"{me.left.type.typeName}\" is incompatible with \"{me.right.type.typeName}\"");
                case DoubleType d:
                    if (me.right.type is IntType)
                        return new DoubleLit((me.left.Eval() as DoubleLit).n * (me.right.Eval() as IntLit).n, me.line, me.column);
                    if (me.right.type is DoubleType)
                        return new DoubleLit((me.left.Eval() as DoubleLit).n * (me.right.Eval() as DoubleLit).n, me.line, me.column);
                    throw new ArgumentException($"Type \"{me.left.type.typeName}\" is incompatible with \"{me.right.type.typeName}\"");
                default:
                    throw new ArgumentException($"Unknown type \"{me.left.type.typeName}\"");
            }
        }

        static IAST EvalDivExpression(DivExpr div)
        {
            switch (div.left.type)
            {
                case IntType i:
                    if (div.right.type is IntType)
                        return new IntLit((div.left.Eval() as IntLit).n / (div.right.Eval() as IntLit).n, div.line, div.column);
                    if (div.right.type is DoubleType)
                        return new DoubleLit((div.left.Eval() as IntLit).n / (div.right.Eval() as DoubleLit).n, div.line, div.column);
                    throw new ArgumentException($"Type \"{div.left.type.typeName}\" is incompatible with \"{div.right.type.typeName}\"");
                case DoubleType d:
                    if (div.right.type is IntType)
                        return new DoubleLit((div.left.Eval() as DoubleLit).n / (div.right.Eval() as IntLit).n, div.line, div.column);
                    if (div.right.type is DoubleType)
                        return new DoubleLit((div.left.Eval() as DoubleLit).n / (div.right.Eval() as DoubleLit).n, div.line, div.column);
                    throw new ArgumentException($"Type \"{div.left.type.typeName}\" is incompatible with \"{div.right.type.typeName}\"");
                default:
                    throw new ArgumentException($"Unknown type \"{div.left.type.typeName}\"");
            }
        }

        static IAST EvalAddExpression(AddExpr add)
        {
            switch (add.left.type)
            {
                case IntType i:
                    if (add.right.type is IntType)
                        return new IntLit((add.left.Eval() as IntLit).n + (add.right.Eval() as IntLit).n, add.line, add.column);
                    if (add.right.type is DoubleType)
                        return new DoubleLit((add.left.Eval() as IntLit).n + (add.right.Eval() as DoubleLit).n, add.line, add.column);
                    throw new ArgumentException($"Type \"{add.left.type.typeName}\" is incompatible with \"{add.right.type.typeName}\"");
                case DoubleType d:
                    if (add.right.type is IntType)
                        return new DoubleLit((add.left.Eval() as DoubleLit).n + (add.right.Eval() as IntLit).n, add.line, add.column);
                    if (add.right.type is DoubleType)
                        return new DoubleLit((add.left.Eval() as DoubleLit).n + (add.right.Eval() as DoubleLit).n, add.line, add.column);
                    throw new ArgumentException($"Type \"{add.left.type.typeName}\" is incompatible with \"{add.right.type.typeName}\"");
                default:
                    throw new ArgumentException($"Unknown type \"{add.left.type.typeName}\"");
            }
        }

        static IAST EvalSubExpression(SubExpr sub)
        {
            switch (sub.left.type)
            {
                case IntType i:
                    if (sub.right.type is IntType)
                        return new IntLit((sub.left.Eval() as IntLit).n - (sub.right.Eval() as IntLit).n, sub.line, sub.column);
                    if (sub.right.type is DoubleType)
                        return new DoubleLit((sub.left.Eval() as IntLit).n - (sub.right.Eval() as DoubleLit).n, sub.line, sub.column);
                    throw new ArgumentException($"Type \"{sub.left.type.typeName}\" is incompatible with \"{sub.right.type.typeName}\"");
                case DoubleType d:
                    if (sub.right.type is IntType)
                        return new DoubleLit((sub.left.Eval() as DoubleLit).n - (sub.right.Eval() as IntLit).n, sub.line, sub.column);
                    if (sub.right.type is DoubleType)
                        return new DoubleLit((sub.left.Eval() as DoubleLit).n - (sub.right.Eval() as DoubleLit).n, sub.line, sub.column);
                    throw new ArgumentException($"Type \"{sub.left.type.typeName}\" is incompatible with \"{sub.right.type.typeName}\"");
                default:
                    throw new ArgumentException($"Unknown type \"{sub.left.type.typeName}\"");
            }
        }

        static IAST EvalEqualityExpression(EqualityExpr eq)
        {
            bool tmp = false;
            IAST left = null;
            IAST right = null;

            switch (eq.left.type)
            {
                case IntType i:
                    if (eq.right.type is IntType)
                        return new BoolLit((eq.left.Eval() as IntLit).n == (eq.right.Eval() as IntLit).n, eq.line, eq.column);
                    if (eq.right.type is DoubleType)
                        return new BoolLit((eq.left.Eval() as IntLit).n == (eq.right.Eval() as DoubleLit).n, eq.line, eq.column);
                    throw new ArgumentException($"Type \"{eq.left.type.typeName}\" is incompatible with \"{eq.right.type.typeName}\"");
                case DoubleType d:
                    if (eq.right.type is IntType)
                        return new BoolLit((eq.left.Eval() as DoubleLit).n == (eq.right.Eval() as IntLit).n, eq.line, eq.column);
                    if (eq.right.type is DoubleType)
                        return new BoolLit((eq.left.Eval() as DoubleLit).n == (eq.right.Eval() as DoubleLit).n, eq.line, eq.column);
                    throw new ArgumentException($"Type \"{eq.left.type.typeName}\" is incompatible with \"{eq.right.type.typeName}\"");
                case BooleanType b:
                    if (!(eq.right.type is BooleanType))
                        throw new ArgumentException($"Type \"{eq.left.type.typeName}\" is incompatible with \"{eq.right.type.typeName}\"");
                    return new BoolLit((eq.left.Eval() as BoolLit).value == (eq.right.Eval() as BoolLit).value, eq.line, eq.column);
                case NullType nullType:
                    tmp = false;

                    if (eq.right.Eval().type is NullType)
                        tmp = true;

                    return new BoolLit(tmp, eq.line, eq.column);
                case StructType structType:
                    tmp = false;
                    left = eq.left.Eval();
                    right = eq.right.Eval();

                    if (left.type is NullType && right.type is NullType)
                        tmp = true;

                    if (left.Equals(right))
                        tmp = true;

                    return new BoolLit(tmp, eq.line, eq.column);
                case ArrayType arrayType:
                    tmp = false;
                    left = eq.left.Eval();
                    right = eq.right.Eval();

                    if (left.type is NullType && right.type is NullType)
                        tmp = true;

                    if (left.Equals(right))
                        tmp = true;

                    return new BoolLit(tmp, eq.line, eq.column);
                default:
                    throw new ArgumentException($"Unknown type \"{eq.left.type.typeName}\"");
            }
        }

        static IAST EvalLessExpression(LessExpr le)
        {
            switch (le.left.type)
            {
                case IntType i:
                    if (le.right.type is IntType)
                    {
                        IAST result = le.equal
                            ? new BoolLit((le.left.Eval() as IntLit).n <= (le.right.Eval() as IntLit).n, le.line, le.column)
                            : new BoolLit((le.left.Eval() as IntLit).n < (le.right.Eval() as IntLit).n, le.line, le.column);
                        return result;
                    }

                    if (le.right.type is DoubleType)
                    {
                        IAST result = le.equal
                            ? new BoolLit((le.left.Eval() as IntLit).n <= (le.right.Eval() as DoubleLit).n, le.line, le.column)
                            : new BoolLit((le.left.Eval() as IntLit).n < (le.right.Eval() as DoubleLit).n, le.line, le.column);
                        return result;
                    }

                    throw new ArgumentException($"Type \"{le.left.type.typeName}\" is incompatible with \"{le.right.type.typeName}\"");
                case DoubleType d:
                    if (le.right.type is IntType)
                    {
                        IAST result = le.equal
                            ? new BoolLit((le.left.Eval() as DoubleLit).n <= (le.right.Eval() as IntLit).n, le.line, le.column)
                            : new BoolLit((le.left.Eval() as DoubleLit).n < (le.right.Eval() as IntLit).n, le.line, le.column);
                        return result;
                    }

                    if (le.right.type is DoubleType)
                    {
                        IAST result = le.equal
                            ? new BoolLit((le.left.Eval() as DoubleLit).n <= (le.right.Eval() as DoubleLit).n, le.line, le.column)
                            : new BoolLit((le.left.Eval() as DoubleLit).n < (le.right.Eval() as DoubleLit).n, le.line, le.column);
                        return result;
                    }

                    throw new ArgumentException($"Type \"{le.left.type.typeName}\" is incompatible with \"{le.right.type.typeName}\"");
                default:
                    throw new ArgumentException($"Unknown type \"{le.left.type.typeName}\"");
            }
        }

        static IAST EvalGreaterExpression(GreaterExpr ge)
        {
            switch (ge.left.type)
            {
                case IntType i:
                    if (ge.right.type is IntType)
                    {
                        IAST result = ge.equal
                            ? new BoolLit((ge.left.Eval() as IntLit).n >= (ge.right.Eval() as IntLit).n, ge.line, ge.column)
                            : new BoolLit((ge.left.Eval() as IntLit).n > (ge.right.Eval() as IntLit).n, ge.line, ge.column);
                        return result;
                    }

                    if (ge.right.type is DoubleType)
                    {
                        IAST result = ge.equal
                            ? new BoolLit((ge.left.Eval() as IntLit).n >= (ge.right.Eval() as DoubleLit).n, ge.line, ge.column)
                            : new BoolLit((ge.left.Eval() as IntLit).n > (ge.right.Eval() as DoubleLit).n, ge.line, ge.column);
                        return result;
                    }

                    throw new ArgumentException($"Type \"{ge.left.type.typeName}\" is incompatible with \"{ge.right.type.typeName}\"");
                case DoubleType d:
                    if (ge.right.type is IntType)
                    {
                        IAST result = ge.equal
                            ? new BoolLit((ge.left.Eval() as IntLit).n >= (ge.right.Eval() as IntLit).n, ge.line, ge.column)
                            : new BoolLit((ge.left.Eval() as IntLit).n > (ge.right.Eval() as IntLit).n, ge.line, ge.column);
                        return result;
                    }

                    if (ge.right.type is DoubleType)
                    {
                        IAST result = ge.equal
                            ? new BoolLit((ge.left.Eval() as IntLit).n >= (ge.right.Eval() as DoubleLit).n, ge.line, ge.column)
                            : new BoolLit((ge.left.Eval() as IntLit).n > (ge.right.Eval() as DoubleLit).n, ge.line, ge.column);
                        return result;
                    }

                    throw new ArgumentException($"Type \"{ge.left.type.typeName}\" is incompatible with \"{ge.right.type.typeName}\"");
                default:
                    throw new ArgumentException($"Unknown type \"{ge.left.type.typeName}\"");
            }
        }

        static IAST EvalIncrementExpr(IncrementExpr increment)
        {
            IAST result;
            IAST variable = EvalValueAccessExpression(increment.variable);

            switch (increment.type)
            {
                case IntType intType:
                    result = new IntLit((variable as IntLit).n + 1, increment.line, increment.column);
                    break;
                case DoubleType doubleType:
                    result = new DoubleLit((variable as DoubleLit).n + 1, increment.line, increment.column);
                    break;
                default:
                    throw new ArgumentException($"Type \"{increment.type.typeName}\" not allowed for Increment");
            }

            switch (increment.variable)
            {
                case VarExpr varExpr:
                    env[varExpr.name] = result;
                    break;
                case ArrayIndexing arrayIndexing:
                    var tmp = new AssignArrayField(arrayIndexing, result, arrayIndexing.line, arrayIndexing.column);
                    tmp.Eval();
                    break;
                case StructPropertyAccess structProperty:
                    var tmp2 = (structProperty.structRef.Eval()) as AST.Struct;

                    if (!(structProperty.prop is VarExpr))
                        throw new ArgumentException("Accessed structproperty is not a variable");

                    tmp2.propValues[(structProperty.prop as VarExpr).name] = result;
                    break;
            }

            if (increment.post)
                return variable;
            else
                return result;
        }

        static IAST EvalDecrementExpr(DecrementExpr decrement)
        {

            IAST result;
            IAST variable = EvalValueAccessExpression(decrement.variable);

            switch (decrement.type)
            {
                case IntType intType:
                    result = new IntLit((variable as IntLit).n - 1, decrement.line, decrement.column);
                    break;
                case DoubleType doubleType:
                    result = new DoubleLit((variable as DoubleLit).n - 1, decrement.line, decrement.column);
                    break;
                default:
                    throw new ArgumentException($"Type \"{decrement.type.typeName}\" not allowed for Increment");
            }

            switch (decrement.variable)
            {
                case VarExpr varExpr:
                    env[varExpr.name] = result;
                    break;
                case ArrayIndexing arrayIndexing:
                    var tmp = new AssignArrayField(arrayIndexing, result, arrayIndexing.line, arrayIndexing.column);
                    tmp.Eval();
                    break;
                case StructPropertyAccess structProperty:
                    var tmp2 = (structProperty.structRef.Eval()) as AST.Struct;

                    if (!(structProperty.prop is VarExpr))
                        throw new ArgumentException("Accessed property is not a variable");
                    tmp2.propValues[(structProperty.prop as VarExpr).name] = result;
                    break;
            }

            if (decrement.post)
                return variable;
            else
                return result;
        }

        static IAST EvalAddAssign(AddAssignStatement addAssign)
        {
            IAST variable;

            switch (addAssign.left.type)
            {
                case IntType intType:
                    variable = env[addAssign.left.name].Eval();
                    return new IntLit((variable as IntLit).n + (addAssign.right.Eval() as IntLit).n, addAssign.line, addAssign.column);
                case DoubleType doubleType:
                    variable = env[addAssign.left.name].Eval();
                    return new DoubleLit((variable as DoubleLit).n + (addAssign.right.Eval() as DoubleLit).n, addAssign.line, addAssign.column);
                default:
                    throw new ArgumentException($"Could not use type \"{addAssign.type.typeName}\" with AddAssignStatement");
            }
        }

        static IAST EvalSubAssign(SubAssignStatement subAssign)
        {
            IAST variable;

            switch (subAssign.left.type)
            {
                case IntType intType:
                    variable = env[subAssign.left.name].Eval();
                    return new IntLit((variable as IntLit).n - (subAssign.right.Eval() as IntLit).n, subAssign.line, subAssign.column);
                case DoubleType doubleType:
                    variable = env[subAssign.left.name].Eval();
                    return new DoubleLit((variable as DoubleLit).n - (subAssign.right.Eval() as DoubleLit).n, subAssign.line, subAssign.column);
                default:
                    throw new ArgumentException($"Could not use type \"{subAssign.type.typeName}\" with SubAssignStatement");
            }
        }

        static IAST EvalMultAssign(MultAssignStatement multAssign)
        {
            IAST variable;

            switch (multAssign.left.type)
            {
                case IntType intType:
                    variable = env[multAssign.left.name].Eval();
                    return new IntLit((variable as IntLit).n * (multAssign.right.Eval() as IntLit).n, multAssign.line, multAssign.column);
                case DoubleType doubleType:
                    variable = env[multAssign.left.name].Eval();
                    return new DoubleLit((variable as DoubleLit).n * (multAssign.right.Eval() as DoubleLit).n, multAssign.line, multAssign.column);
                default:
                    throw new ArgumentException($"Could not use type \"{multAssign.type.typeName}\" with MultAssignStatement");
            }
        }

        static IAST EvalDivAssign(DivAssignStatement divAssign)
        {
            IAST variable;

            switch (divAssign.left.type)
            {
                case IntType intType:
                    variable = env[divAssign.left.name].Eval();
                    return new IntLit((variable as IntLit).n / (divAssign.right.Eval() as IntLit).n, divAssign.line, divAssign.column);
                case DoubleType doubleType:
                    variable = env[divAssign.left.name].Eval();
                    return new DoubleLit((variable as DoubleLit).n / (divAssign.right.Eval() as DoubleLit).n, divAssign.line, divAssign.column);
                default:
                    throw new ArgumentException($"Could not use type \"{divAssign.type.typeName}\" with DivAssignStatement");
            }
        }

        static IAST EvalNotEqualExpression(NotEqualExpr notEqual)
        {
            bool tmp = false;
            IAST left = null;
            IAST right = null;

            switch (notEqual.left.type)
            {
                case IntType i:
                    if (notEqual.right.type is IntType)
                        return new BoolLit((notEqual.left.Eval() as IntLit).n != (notEqual.right.Eval() as IntLit).n, notEqual.line, notEqual.column);
                    if (notEqual.right.type is DoubleType)
                        return new BoolLit((notEqual.left.Eval() as IntLit).n != (notEqual.right.Eval() as DoubleLit).n, notEqual.line, notEqual.column);
                    throw new ArgumentException($"Type \"{notEqual.left.type.typeName}\" is incompatible with \"{notEqual.right.type.typeName}\"");
                case DoubleType d:
                    if (notEqual.right.type is IntType)
                        return new BoolLit((notEqual.left.Eval() as DoubleLit).n != (notEqual.right.Eval() as IntLit).n, notEqual.line, notEqual.column);
                    if (notEqual.right.type is DoubleType)
                        return new BoolLit((notEqual.left.Eval() as DoubleLit).n != (notEqual.right.Eval() as DoubleLit).n, notEqual.line, notEqual.column);
                    throw new ArgumentException($"Type \"{notEqual.left.type.typeName}\" is incompatible with \"{notEqual.right.type.typeName}\"");
                case BooleanType b:
                    if (!(notEqual.right.type is BooleanType))
                        throw new ArgumentException($"Type \"{notEqual.left.type.typeName}\" is incompatible with \"{notEqual.right.type.typeName}\"");
                    return new BoolLit((notEqual.left.Eval() as BoolLit).value != (notEqual.right.Eval() as BoolLit).value, notEqual.line, notEqual.column);
                case NullType nullType:
                    tmp = false;

                    if (!(notEqual.right.Eval().type is NullType))
                        tmp = true;

                    return new BoolLit(tmp, notEqual.line, notEqual.column);
                case StructType structType:
                    tmp = false;
                    left = notEqual.left.Eval();
                    right = notEqual.right.Eval();

                    if (!(left.type is NullType && right.type is NullType))
                        tmp = true;

                    if (!(left.Equals(right)))
                        tmp = true;

                    return new BoolLit(tmp, notEqual.line, notEqual.column);
                case ArrayType arrayType:
                    tmp = false;
                    left = notEqual.left.Eval();
                    right = notEqual.right.Eval();

                    if (!(left.type is NullType && right.type is NullType))
                        tmp = true;

                    if (!left.Equals(right))
                        tmp = true;

                    return new BoolLit(tmp, notEqual.line, notEqual.column);
                default:
                    throw new ArgumentException($"Unknown type \"{notEqual.left.type.typeName}\"");
            }
        }

        static void EvalPrintStatement(PrintStatement print)
        {
            string result = "";
            switch (print.value.type)
            {
                case IntType it:
                    var tmp = print.value.Eval() as IntLit;
                    result = tmp.n.ToString() ?? "";
                    break;
                case DoubleType dt:
                    var tmp2 = print.value.Eval() as DoubleLit;
                    result = tmp2.n.ToString() ?? "";
                    break;
                case BooleanType bt:
                    var tmp3 = print.value.Eval() as BoolLit;
                    result = tmp3.value.ToString() ?? "";
                    break;
                default:
                    throw new ArgumentException($"Print does not support type {print.value.type.typeName}");
            }

            Console.WriteLine(result);
        }

        static IAST EvalRefTypeCreationStatement(RefTypeCreationStatement refType)
        {
            if (refType.createdReftype is Array)
                return CreateArray(refType.createdReftype as Array);
            if (refType.createdReftype is Struct)
                return CreateStruct(refType.createdReftype as Struct);

            throw new ArgumentException($"Unknown type for reference type creation: \"{refType.createdReftype.type}\"");
        }

        static IAST CreateArray(Array array)
        {
            long size = (array.size.Eval() as IntLit).n ?? -1;

            if (size < 0)
                throw new ArgumentException("The length of an array has to be positive");

            array.values = new IAST[size];

            return array;
        }

        static IAST CreateStruct(Struct @struct)
        {
            @struct.propValues = new Dictionary<string, IAST>();
            StructDefinition structDef = structs[@struct.name];

            foreach (var prop in structDef.properties)
                @struct.propValues[prop.name] = null;

            return @struct;
        }

        static IAST EvalIntArray(IntArray intArray)
        {
            if (intArray.values == null)
                throw new ArgumentException("Array is not initialized");

            return intArray;
        }

        static IAST EvalDoubleArray(DoubleArray doubleArray)
        {
            if (doubleArray.values == null)
                throw new ArgumentException("Array is not initialized");

            return doubleArray;
        }

        static IAST EvalBoolArray(BoolArray boolArray)
        {
            if (boolArray.values == null)
                throw new ArgumentException("Array is not initialized");

            return boolArray;
        }

        static IAST EvalArrayIndexing(ArrayIndexing arrayIndexing)
        {
            Array array = arrayIndexing.left.Eval() as Array;
            long index = (arrayIndexing.right.Eval() as IntLit).n ?? -1;
            long arraySize = (array.size.Eval() as IntLit).n ?? -1;

            if (index < 0)
                throw new ArgumentException("The index of an array must be initialized");

            if (index >= arraySize)
                throw new ArgumentException($"Index {index} out of range {(arraySize)}");

            return array.values[index];
        }

        static void EvalAssignArrayField(AssignArrayField assignArrayField)
        {
            Array array = assignArrayField.arrayIndex.left.Eval() as Array;
            long index = (assignArrayField.arrayIndex.right.Eval() as IntLit).n ?? -1;
            long arraySize = (array.size.Eval() as IntLit).n ?? -1;

            if (index < 0)
                throw new ArgumentException("The index of an array must be initialized");

            if (index >= arraySize)
                throw new ArgumentException($"Index {index} out of range {arraySize}");

            IAST value = assignArrayField.value.Eval();

            array.values[index] = value;
        }

        static IAST EvalStructPropertyAccess(StructPropertyAccess structPropertyAccess)
        {
            Struct @struct = structPropertyAccess.structRef.Eval() as Struct;
            structEnv = @struct.propValues;
            var result = structPropertyAccess.prop.Eval();

            structEnv = null;

            return result;
        }

        static void EvalAssignStructProperty(AssignStructProperty assignStruct)
        {
            StructPropertyAccess innerStruct = GetPropRef(assignStruct.structProp, out long index);
            Struct @struct = innerStruct.structRef.Eval() as Struct;
            string propName = "";

            if (index >= 0)
                propName = ((innerStruct.prop as ArrayIndexing).left as VarExpr).name;
            else
                propName = (innerStruct.prop as VarExpr).name;

            structEnv = null;

            if (index >= 0)
                (@struct.propValues[propName] as Array).values[index] = assignStruct.val.Eval();
            else
                @struct.propValues[propName] = assignStruct.val.Eval();
        }

        static IAST EvalValueAccessExpression(ValueAccessExpression valueAccess)
        {
            switch (valueAccess)
            {
                case VarExpr varExpr: return varExpr.Eval();
                case ArrayIndexing arrayIndexing: return arrayIndexing.Eval();
                case StructPropertyAccess structProperty: return structProperty.Eval();
                default:
                    throw new ArgumentException($"Unknown valueAccessExpression; On line {valueAccess.line}:{valueAccess.column}");
            }
        }

        static StructPropertyAccess GetPropRef(StructPropertyAccess val, out long index)
        {
            index = -1;
            if (val.prop is VarExpr)
                return val;
            else if (val.prop is ArrayIndexing arrayIndexing)
            {
                index = (arrayIndexing.right.Eval() as IntLit).n ?? -1;
                return val;
            }
            else
            {
                structEnv = (val.structRef.Eval() as Struct).propValues;

                var result = GetPropRef(val.prop as StructPropertyAccess, out long i);
                index = i;

                return result;
            }
        }
    }
}