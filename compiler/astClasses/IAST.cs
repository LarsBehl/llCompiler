using System;
using System.Collections.Generic;
using LL.Types;

namespace LL.AST
{
    public abstract class IAST
    {
        public LL.Types.Type Type { get; set; }
        public int Line { get; set; }
        public int Column { get; set; }

        public static string CurrentFile;

        // needed for evaluation
        private static Dictionary<string, FunctionDefinition> Funs;
        private static Dictionary<string, StructDefinition> Structs;
        private static Dictionary<string, IAST> StructEnv;
        private static Dictionary<string, IAST> Env = new Dictionary<string, IAST>();

        public IAST(LL.Types.Type type, int line, int column)
        {
            this.Type = type;
            this.Line = line;
            this.Column = column;
        }

        public IAST Eval()
        {
            switch (this)
            {
                case IntLit i: return i;
                case DoubleLit d: return d;
                case BoolLit b: return b;
                case CharLit charLit: return charLit;
                case MultExpr me:
                    return EvalMultExpression(me);
                case AddExpr add:
                    return EvalAddExpression(add);
                case SubExpr sub:
                    return EvalSubExpression(sub);
                case DivExpr div:
                    return EvalDivExpression(div);
                case VarExpr varExpr:
                    if (StructEnv != null)
                        return StructEnv[varExpr.Name];
                    var envVar = Env[varExpr.Name];

                    switch (envVar)
                    {
                        case DoubleLit dl:
                            if (dl.Value == null)
                                throw new ArgumentException($"Variable \"{varExpr.Name}\" is not initialized");
                            break;
                        case IntLit il:
                            if (il.Value == null)
                                throw new ArgumentException($"Variable \"{varExpr.Name}\" is not initialized");
                            break;
                        case BoolLit bl:
                            if (bl.Value == null)
                                throw new ArgumentException($"Variable \"{varExpr.Name}\" is not initialized");
                            break;
                        case CharLit cl:
                            if(cl.Value == null)
                                throw new ArgumentException($"Variable \"{varExpr.Name}\" is not initialized");
                            break;
                        case IntArray intArray:
                            if (((intArray.Size.Eval() as IntLit).Value ?? -1) < 0)
                                throw new ArgumentException($"Variable \"{varExpr.Name}\" is not initialized");
                            break;
                        case DoubleArray doubleArray:
                            if (((doubleArray.Size.Eval() as IntLit).Value ?? -1) < 0)
                                throw new ArgumentException($"Variable \"{varExpr.Name}\" is not initialized");
                            break;
                        case BoolArray boolArray:
                            if (((boolArray.Size.Eval() as IntLit).Value ?? -1) < 0)
                                throw new ArgumentException($"Variable \"{varExpr.Name}\" is not initialized");
                            break;
                        default:
                            break;
                    }

                    return envVar.Eval();
                case AssignStatement assign:
                    var tmp = assign.Value.Eval();
                    Env[assign.Variable.Name] = tmp;
                    return null;
                case BlockStatement block:
                    IAST resultBlock = null;

                    foreach (var comp in block.Body)
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

                        if (comp is WhileStatement && !(comp.Type is WhileStatementType))
                        {
                            resultBlock = comp.Eval();
                            break;
                        }

                        comp.Eval();
                    }

                    return resultBlock;
                case ReturnStatement returnExpr:
                    return returnExpr.ReturnValue?.Eval() ?? null;
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
                    FunctionDefinition fDef = Funs[funCall.FunctionName];
                    var newEnv = new Dictionary<string, IAST>(fDef.FunctionEnv);

                    for (int k = 0; k < funCall.Args.Count; k++)
                    {
                        newEnv[fDef.Args[k].Name] = funCall.Args[k].Eval();
                    }

                    var oldEnv = Env;
                    Env = newEnv;

                    var result = fDef.Body.Eval();
                    Env = oldEnv;

                    return result;
                case IfStatement ifStatement:
                    if ((ifStatement.Cond.Eval() as BoolLit).Value ?? false)
                        return ifStatement.IfBody.Eval();
                    return ifStatement.ElseBody?.Eval() ?? null;
                case WhileStatement whileStatement:
                    IAST result_while = null;
                    while ((whileStatement.Condition.Eval() as BoolLit).Value ?? false)
                    {
                        result_while = whileStatement.Body.Eval();
                        if (result_while != null && !(result_while.Type is BlockStatementType))
                            break;
                    }

                    return result_while;
                case IncrementExpr increment:
                    return EvalIncrementExpr(increment);
                case DecrementExpr decrement:
                    return EvalDecrementExpr(decrement);
                case AddAssignStatement addAssign:
                    Env[addAssign.Left.Name] = EvalAddAssign(addAssign);
                    return null;
                case SubAssignStatement subAssign:
                    Env[subAssign.Left.Name] = EvalSubAssign(subAssign);
                    return null;
                case MultAssignStatement multAssign:
                    Env[multAssign.Left.Name] = EvalMultAssign(multAssign);
                    return null;
                case DivAssignStatement divAssign:
                    Env[divAssign.Left.Name] = EvalDivAssign(divAssign);
                    return null;
                case NotExpr notExpr:
                    return new BoolLit(!(notExpr.Value.Eval() as BoolLit).Value, notExpr.Line, notExpr.Column);
                case AndExpr andExpr:
                    return new BoolLit(
                        ((andExpr.Left.Eval() as BoolLit).Value ?? false)
                        && ((andExpr.Right.Eval() as BoolLit).Value ?? false), andExpr.Line, andExpr.Column);
                case OrExpr orExpr:
                    return new BoolLit(
                        ((orExpr.Left.Eval() as BoolLit).Value ?? false)
                        || ((orExpr.Right.Eval() as BoolLit).Value ?? false), orExpr.Line, orExpr.Column);
                case NotEqualExpr notEqualExpr:
                    return EvalNotEqualExpression(notEqualExpr);
                case ProgramNode programNode:
                    foreach (var f in programNode.FunDefs)
                        f.Value.Eval();
                    
                    Funs = programNode.FunDefs;

                    foreach(var s in programNode.StructDefs)
                        s.Value.Eval();

                    Structs = programNode.StructDefs;

                    if(programNode.CompositUnit is not null)
                    {
                        Env = programNode.Env;
                        return programNode.CompositUnit.Eval();
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
                case ModExpr modExpr:
                    return EvalModExpr(modExpr);
                default:
                    throw new ArgumentException("Unknown Ast Object");
            }
        }

        private IAST EvalMultExpression(MultExpr me)
        {
            switch (me.Left.Type)
            {
                case IntType i:
                    if (me.Right.Type is IntType)
                        return new IntLit((me.Left.Eval() as IntLit).Value * (me.Right.Eval() as IntLit).Value, me.Line, me.Column);
                    if (me.Right.Type is DoubleType)
                        return new DoubleLit((me.Left.Eval() as IntLit).Value * (me.Right.Eval() as DoubleLit).Value, me.Line, me.Column);
                    throw new ArgumentException($"Type \"{me.Left.Type.TypeName}\" is incompatible with \"{me.Right.Type.TypeName}\"");
                case DoubleType d:
                    if (me.Right.Type is IntType)
                        return new DoubleLit((me.Left.Eval() as DoubleLit).Value * (me.Right.Eval() as IntLit).Value, me.Line, me.Column);
                    if (me.Right.Type is DoubleType)
                        return new DoubleLit((me.Left.Eval() as DoubleLit).Value * (me.Right.Eval() as DoubleLit).Value, me.Line, me.Column);
                    throw new ArgumentException($"Type \"{me.Left.Type.TypeName}\" is incompatible with \"{me.Right.Type.TypeName}\"");
                default:
                    throw new ArgumentException($"Unknown type \"{me.Left.Type.TypeName}\"");
            }
        }

        private IAST EvalDivExpression(DivExpr div)
        {
            switch (div.Left.Type)
            {
                case IntType i:
                    if (div.Right.Type is IntType)
                        return new IntLit((div.Left.Eval() as IntLit).Value / (div.Right.Eval() as IntLit).Value, div.Line, div.Column);
                    if (div.Right.Type is DoubleType)
                        return new DoubleLit((div.Left.Eval() as IntLit).Value / (div.Right.Eval() as DoubleLit).Value, div.Line, div.Column);
                    throw new ArgumentException($"Type \"{div.Left.Type.TypeName}\" is incompatible with \"{div.Right.Type.TypeName}\"");
                case DoubleType d:
                    if (div.Right.Type is IntType)
                        return new DoubleLit((div.Left.Eval() as DoubleLit).Value / (div.Right.Eval() as IntLit).Value, div.Line, div.Column);
                    if (div.Right.Type is DoubleType)
                        return new DoubleLit((div.Left.Eval() as DoubleLit).Value / (div.Right.Eval() as DoubleLit).Value, div.Line, div.Column);
                    throw new ArgumentException($"Type \"{div.Left.Type.TypeName}\" is incompatible with \"{div.Right.Type.TypeName}\"");
                default:
                    throw new ArgumentException($"Unknown type \"{div.Left.Type.TypeName}\"");
            }
        }

        private IAST EvalAddExpression(AddExpr add)
        {
            switch (add.Left.Type)
            {
                case IntType i:
                    if (add.Right.Type is IntType)
                        return new IntLit((add.Left.Eval() as IntLit).Value + (add.Right.Eval() as IntLit).Value, add.Line, add.Column);
                    if (add.Right.Type is DoubleType)
                        return new DoubleLit((add.Left.Eval() as IntLit).Value + (add.Right.Eval() as DoubleLit).Value, add.Line, add.Column);
                    throw new ArgumentException($"Type \"{add.Left.Type.TypeName}\" is incompatible with \"{add.Right.Type.TypeName}\"");
                case DoubleType d:
                    if (add.Right.Type is IntType)
                        return new DoubleLit((add.Left.Eval() as DoubleLit).Value + (add.Right.Eval() as IntLit).Value, add.Line, add.Column);
                    if (add.Right.Type is DoubleType)
                        return new DoubleLit((add.Left.Eval() as DoubleLit).Value + (add.Right.Eval() as DoubleLit).Value, add.Line, add.Column);
                    throw new ArgumentException($"Type \"{add.Left.Type.TypeName}\" is incompatible with \"{add.Right.Type.TypeName}\"");
                default:
                    throw new ArgumentException($"Unknown type \"{add.Left.Type.TypeName}\"");
            }
        }

        private IAST EvalSubExpression(SubExpr sub)
        {
            switch (sub.Left.Type)
            {
                case IntType i:
                    if (sub.Right.Type is IntType)
                        return new IntLit((sub.Left.Eval() as IntLit).Value - (sub.Right.Eval() as IntLit).Value, sub.Line, sub.Column);
                    if (sub.Right.Type is DoubleType)
                        return new DoubleLit((sub.Left.Eval() as IntLit).Value - (sub.Right.Eval() as DoubleLit).Value, sub.Line, sub.Column);
                    throw new ArgumentException($"Type \"{sub.Left.Type.TypeName}\" is incompatible with \"{sub.Right.Type.TypeName}\"");
                case DoubleType d:
                    if (sub.Right.Type is IntType)
                        return new DoubleLit((sub.Left.Eval() as DoubleLit).Value - (sub.Right.Eval() as IntLit).Value, sub.Line, sub.Column);
                    if (sub.Right.Type is DoubleType)
                        return new DoubleLit((sub.Left.Eval() as DoubleLit).Value - (sub.Right.Eval() as DoubleLit).Value, sub.Line, sub.Column);
                    throw new ArgumentException($"Type \"{sub.Left.Type.TypeName}\" is incompatible with \"{sub.Right.Type.TypeName}\"");
                default:
                    throw new ArgumentException($"Unknown type \"{sub.Left.Type.TypeName}\"");
            }
        }

        private IAST EvalEqualityExpression(EqualityExpr eq)
        {
            bool tmp = false;
            IAST left = null;
            IAST right = null;

            switch (eq.Left.Type)
            {
                case IntType i:
                    if (eq.Right.Type is IntType)
                        return new BoolLit((eq.Left.Eval() as IntLit).Value == (eq.Right.Eval() as IntLit).Value, eq.Line, eq.Column);
                    if (eq.Right.Type is DoubleType)
                        return new BoolLit((eq.Left.Eval() as IntLit).Value == (eq.Right.Eval() as DoubleLit).Value, eq.Line, eq.Column);
                    throw new ArgumentException($"Type \"{eq.Left.Type.TypeName}\" is incompatible with \"{eq.Right.Type.TypeName}\"");
                case DoubleType d:
                    if (eq.Right.Type is IntType)
                        return new BoolLit((eq.Left.Eval() as DoubleLit).Value == (eq.Right.Eval() as IntLit).Value, eq.Line, eq.Column);
                    if (eq.Right.Type is DoubleType)
                        return new BoolLit((eq.Left.Eval() as DoubleLit).Value == (eq.Right.Eval() as DoubleLit).Value, eq.Line, eq.Column);
                    throw new ArgumentException($"Type \"{eq.Left.Type.TypeName}\" is incompatible with \"{eq.Right.Type.TypeName}\"");
                case BooleanType b:
                    if (!(eq.Right.Type is BooleanType))
                        throw new ArgumentException($"Type \"{eq.Left.Type.TypeName}\" is incompatible with \"{eq.Right.Type.TypeName}\"");
                    return new BoolLit((eq.Left.Eval() as BoolLit).Value == (eq.Right.Eval() as BoolLit).Value, eq.Line, eq.Column);
                case CharType charType:
                    if(charType != eq.Right.Type)
                        throw new ArgumentException($"Type \"{eq.Left.Type.TypeName}\" is incompatible with \"{eq.Right.Type.TypeName}\"");
                    return new BoolLit((eq.Left.Eval() as CharLit).Value == (eq.Right.Eval() as CharLit).Value, eq.Line, eq.Column);
                case NullType nullType:
                    tmp = false;

                    if (eq.Right.Eval().Type is NullType)
                        tmp = true;

                    return new BoolLit(tmp, eq.Line, eq.Column);
                case StructType structType:
                    tmp = false;
                    left = eq.Left.Eval();
                    right = eq.Right.Eval();

                    if (left.Type is NullType && right.Type is NullType)
                        tmp = true;

                    if (left.Equals(right))
                        tmp = true;

                    return new BoolLit(tmp, eq.Line, eq.Column);
                case ArrayType arrayType:
                    tmp = false;
                    left = eq.Left.Eval();
                    right = eq.Right.Eval();

                    if (left.Type is NullType && right.Type is NullType)
                        tmp = true;

                    if (left.Equals(right))
                        tmp = true;

                    return new BoolLit(tmp, eq.Line, eq.Column);
                default:
                    throw new ArgumentException($"Unknown type \"{eq.Left.Type.TypeName}\"");
            }
        }

        private IAST EvalLessExpression(LessExpr le)
        {
            switch (le.Left.Type)
            {
                case IntType i:
                    if (le.Right.Type is IntType)
                    {
                        IAST result = le.Equal
                            ? new BoolLit((le.Left.Eval() as IntLit).Value <= (le.Right.Eval() as IntLit).Value, le.Line, le.Column)
                            : new BoolLit((le.Left.Eval() as IntLit).Value < (le.Right.Eval() as IntLit).Value, le.Line, le.Column);
                        return result;
                    }

                    if (le.Right.Type is DoubleType)
                    {
                        IAST result = le.Equal
                            ? new BoolLit((le.Left.Eval() as IntLit).Value <= (le.Right.Eval() as DoubleLit).Value, le.Line, le.Column)
                            : new BoolLit((le.Left.Eval() as IntLit).Value < (le.Right.Eval() as DoubleLit).Value, le.Line, le.Column);
                        return result;
                    }

                    throw new ArgumentException($"Type \"{le.Left.Type.TypeName}\" is incompatible with \"{le.Right.Type.TypeName}\"");
                case DoubleType d:
                    if (le.Right.Type is IntType)
                    {
                        IAST result = le.Equal
                            ? new BoolLit((le.Left.Eval() as DoubleLit).Value <= (le.Right.Eval() as IntLit).Value, le.Line, le.Column)
                            : new BoolLit((le.Left.Eval() as DoubleLit).Value < (le.Right.Eval() as IntLit).Value, le.Line, le.Column);
                        return result;
                    }

                    if (le.Right.Type is DoubleType)
                    {
                        IAST result = le.Equal
                            ? new BoolLit((le.Left.Eval() as DoubleLit).Value <= (le.Right.Eval() as DoubleLit).Value, le.Line, le.Column)
                            : new BoolLit((le.Left.Eval() as DoubleLit).Value < (le.Right.Eval() as DoubleLit).Value, le.Line, le.Column);
                        return result;
                    }

                    throw new ArgumentException($"Type \"{le.Left.Type.TypeName}\" is incompatible with \"{le.Right.Type.TypeName}\"");
                default:
                    throw new ArgumentException($"Unknown type \"{le.Left.Type.TypeName}\"");
            }
        }

        private IAST EvalGreaterExpression(GreaterExpr ge)
        {
            switch (ge.Left.Type)
            {
                case IntType i:
                    if (ge.Right.Type is IntType)
                    {
                        IAST result = ge.Equal
                            ? new BoolLit((ge.Left.Eval() as IntLit).Value >= (ge.Right.Eval() as IntLit).Value, ge.Line, ge.Column)
                            : new BoolLit((ge.Left.Eval() as IntLit).Value > (ge.Right.Eval() as IntLit).Value, ge.Line, ge.Column);
                        return result;
                    }

                    if (ge.Right.Type is DoubleType)
                    {
                        IAST result = ge.Equal
                            ? new BoolLit((ge.Left.Eval() as IntLit).Value >= (ge.Right.Eval() as DoubleLit).Value, ge.Line, ge.Column)
                            : new BoolLit((ge.Left.Eval() as IntLit).Value > (ge.Right.Eval() as DoubleLit).Value, ge.Line, ge.Column);
                        return result;
                    }

                    throw new ArgumentException($"Type \"{ge.Left.Type.TypeName}\" is incompatible with \"{ge.Right.Type.TypeName}\"");
                case DoubleType d:
                    if (ge.Right.Type is IntType)
                    {
                        IAST result = ge.Equal
                            ? new BoolLit((ge.Left.Eval() as IntLit).Value >= (ge.Right.Eval() as IntLit).Value, ge.Line, ge.Column)
                            : new BoolLit((ge.Left.Eval() as IntLit).Value > (ge.Right.Eval() as IntLit).Value, ge.Line, ge.Column);
                        return result;
                    }

                    if (ge.Right.Type is DoubleType)
                    {
                        IAST result = ge.Equal
                            ? new BoolLit((ge.Left.Eval() as IntLit).Value >= (ge.Right.Eval() as DoubleLit).Value, ge.Line, ge.Column)
                            : new BoolLit((ge.Left.Eval() as IntLit).Value > (ge.Right.Eval() as DoubleLit).Value, ge.Line, ge.Column);
                        return result;
                    }

                    throw new ArgumentException($"Type \"{ge.Left.Type.TypeName}\" is incompatible with \"{ge.Right.Type.TypeName}\"");
                default:
                    throw new ArgumentException($"Unknown type \"{ge.Left.Type.TypeName}\"");
            }
        }

        private IAST EvalIncrementExpr(IncrementExpr increment)
        {
            IAST result;
            IAST variable = EvalValueAccessExpression(increment.Variable);

            switch (increment.Type)
            {
                case IntType intType:
                    result = new IntLit((variable as IntLit).Value + 1, increment.Line, increment.Column);
                    break;
                case DoubleType doubleType:
                    result = new DoubleLit((variable as DoubleLit).Value + 1, increment.Line, increment.Column);
                    break;
                default:
                    throw new ArgumentException($"Type \"{increment.Type.TypeName}\" not allowed for Increment");
            }

            switch (increment.Variable)
            {
                case VarExpr varExpr:
                    Env[varExpr.Name] = result;
                    break;
                case ArrayIndexing arrayIndexing:
                    var tmp = new AssignArrayField(arrayIndexing, result, arrayIndexing.Line, arrayIndexing.Column);
                    tmp.Eval();
                    break;
                case StructPropertyAccess structProperty:
                    var tmp2 = (structProperty.StructRef.Eval()) as AST.Struct;

                    if (!(structProperty.Prop is VarExpr))
                        throw new ArgumentException("Accessed structproperty is not a variable");

                    tmp2.PropValues[(structProperty.Prop as VarExpr).Name] = result;
                    break;
            }

            if (increment.Post)
                return variable;
            else
                return result;
        }

        private IAST EvalDecrementExpr(DecrementExpr decrement)
        {

            IAST result;
            IAST variable = EvalValueAccessExpression(decrement.Variable);

            switch (decrement.Type)
            {
                case IntType intType:
                    result = new IntLit((variable as IntLit).Value - 1, decrement.Line, decrement.Column);
                    break;
                case DoubleType doubleType:
                    result = new DoubleLit((variable as DoubleLit).Value - 1, decrement.Line, decrement.Column);
                    break;
                default:
                    throw new ArgumentException($"Type \"{decrement.Type.TypeName}\" not allowed for Increment");
            }

            switch (decrement.Variable)
            {
                case VarExpr varExpr:
                    Env[varExpr.Name] = result;
                    break;
                case ArrayIndexing arrayIndexing:
                    var tmp = new AssignArrayField(arrayIndexing, result, arrayIndexing.Line, arrayIndexing.Column);
                    tmp.Eval();
                    break;
                case StructPropertyAccess structProperty:
                    var tmp2 = (structProperty.StructRef.Eval()) as AST.Struct;

                    if (!(structProperty.Prop is VarExpr))
                        throw new ArgumentException("Accessed property is not a variable");
                    tmp2.PropValues[(structProperty.Prop as VarExpr).Name] = result;
                    break;
            }

            if (decrement.Post)
                return variable;
            else
                return result;
        }

        private IAST EvalAddAssign(AddAssignStatement addAssign)
        {
            IAST variable;

            switch (addAssign.Left.Type)
            {
                case IntType intType:
                    variable = Env[addAssign.Left.Name].Eval();
                    return new IntLit((variable as IntLit).Value + (addAssign.Right.Eval() as IntLit).Value, addAssign.Line, addAssign.Column);
                case DoubleType doubleType:
                    variable = Env[addAssign.Left.Name].Eval();
                    return new DoubleLit((variable as DoubleLit).Value + (addAssign.Right.Eval() as DoubleLit).Value, addAssign.Line, addAssign.Column);
                default:
                    throw new ArgumentException($"Could not use type \"{addAssign.Type.TypeName}\" with AddAssignStatement");
            }
        }

        private IAST EvalSubAssign(SubAssignStatement subAssign)
        {
            IAST variable;

            switch (subAssign.Left.Type)
            {
                case IntType intType:
                    variable = Env[subAssign.Left.Name].Eval();
                    return new IntLit((variable as IntLit).Value - (subAssign.Right.Eval() as IntLit).Value, subAssign.Line, subAssign.Column);
                case DoubleType doubleType:
                    variable = Env[subAssign.Left.Name].Eval();
                    return new DoubleLit((variable as DoubleLit).Value - (subAssign.Right.Eval() as DoubleLit).Value, subAssign.Line, subAssign.Column);
                default:
                    throw new ArgumentException($"Could not use type \"{subAssign.Type.TypeName}\" with SubAssignStatement");
            }
        }

        private IAST EvalMultAssign(MultAssignStatement multAssign)
        {
            IAST variable;

            switch (multAssign.Left.Type)
            {
                case IntType intType:
                    variable = Env[multAssign.Left.Name].Eval();
                    return new IntLit((variable as IntLit).Value * (multAssign.Right.Eval() as IntLit).Value, multAssign.Line, multAssign.Column);
                case DoubleType doubleType:
                    variable = Env[multAssign.Left.Name].Eval();
                    return new DoubleLit((variable as DoubleLit).Value * (multAssign.Right.Eval() as DoubleLit).Value, multAssign.Line, multAssign.Column);
                default:
                    throw new ArgumentException($"Could not use type \"{multAssign.Type.TypeName}\" with MultAssignStatement");
            }
        }

        private IAST EvalDivAssign(DivAssignStatement divAssign)
        {
            IAST variable;

            switch (divAssign.Left.Type)
            {
                case IntType intType:
                    variable = Env[divAssign.Left.Name].Eval();
                    return new IntLit((variable as IntLit).Value / (divAssign.Right.Eval() as IntLit).Value, divAssign.Line, divAssign.Column);
                case DoubleType doubleType:
                    variable = Env[divAssign.Left.Name].Eval();
                    return new DoubleLit((variable as DoubleLit).Value / (divAssign.Right.Eval() as DoubleLit).Value, divAssign.Line, divAssign.Column);
                default:
                    throw new ArgumentException($"Could not use type \"{divAssign.Type.TypeName}\" with DivAssignStatement");
            }
        }

        private IAST EvalNotEqualExpression(NotEqualExpr notEqual)
        {
            bool tmp = false;
            IAST left = null;
            IAST right = null;

            switch (notEqual.Left.Type)
            {
                case IntType i:
                    if (notEqual.Right.Type is IntType)
                        return new BoolLit((notEqual.Left.Eval() as IntLit).Value != (notEqual.Right.Eval() as IntLit).Value, notEqual.Line, notEqual.Column);
                    if (notEqual.Right.Type is DoubleType)
                        return new BoolLit((notEqual.Left.Eval() as IntLit).Value != (notEqual.Right.Eval() as DoubleLit).Value, notEqual.Line, notEqual.Column);
                    throw new ArgumentException($"Type \"{notEqual.Left.Type.TypeName}\" is incompatible with \"{notEqual.Right.Type.TypeName}\"");
                case DoubleType d:
                    if (notEqual.Right.Type is IntType)
                        return new BoolLit((notEqual.Left.Eval() as DoubleLit).Value != (notEqual.Right.Eval() as IntLit).Value, notEqual.Line, notEqual.Column);
                    if (notEqual.Right.Type is DoubleType)
                        return new BoolLit((notEqual.Left.Eval() as DoubleLit).Value != (notEqual.Right.Eval() as DoubleLit).Value, notEqual.Line, notEqual.Column);
                    throw new ArgumentException($"Type \"{notEqual.Left.Type.TypeName}\" is incompatible with \"{notEqual.Right.Type.TypeName}\"");
                case BooleanType b:
                    if (!(notEqual.Right.Type is BooleanType))
                        throw new ArgumentException($"Type \"{notEqual.Left.Type.TypeName}\" is incompatible with \"{notEqual.Right.Type.TypeName}\"");
                    return new BoolLit((notEqual.Left.Eval() as BoolLit).Value != (notEqual.Right.Eval() as BoolLit).Value, notEqual.Line, notEqual.Column);
                case CharType charType:
                    if(charType != notEqual.Right.Type)
                        throw new ArgumentException($"Type \"{charType.TypeName}\" is incompatible with \"{notEqual.Right.Type.TypeName}\"");
                    return new BoolLit((notEqual.Left.Eval() as CharLit).Value != (notEqual.Right.Eval() as CharLit).Value, notEqual.Line, notEqual.Column);
                case NullType nullType:
                    tmp = false;

                    if (!(notEqual.Right.Eval().Type is NullType))
                        tmp = true;

                    return new BoolLit(tmp, notEqual.Line, notEqual.Column);
                case StructType structType:
                    tmp = false;
                    left = notEqual.Left.Eval();
                    right = notEqual.Right.Eval();

                    if (!(left.Type is NullType && right.Type is NullType))
                        tmp = true;

                    if (!(left.Equals(right)))
                        tmp = true;

                    return new BoolLit(tmp, notEqual.Line, notEqual.Column);
                case ArrayType arrayType:
                    tmp = false;
                    left = notEqual.Left.Eval();
                    right = notEqual.Right.Eval();

                    if (!(left.Type is NullType && right.Type is NullType))
                        tmp = true;

                    if (!left.Equals(right))
                        tmp = true;

                    return new BoolLit(tmp, notEqual.Line, notEqual.Column);
                default:
                    throw new ArgumentException($"Unknown type \"{notEqual.Left.Type.TypeName}\"");
            }
        }

        private void EvalPrintStatement(PrintStatement print)
        {
            string result = "";
            switch (print.Value.Type)
            {
                case IntType it:
                    var tmp = print.Value.Eval() as IntLit;
                    result = tmp.Value.ToString() ?? "";
                    break;
                case DoubleType dt:
                    var tmp2 = print.Value.Eval() as DoubleLit;
                    result = tmp2.Value.ToString() ?? "";
                    break;
                case BooleanType bt:
                    var tmp3 = print.Value.Eval() as BoolLit;
                    result = tmp3.Value.ToString() ?? "";
                    break;
                case CharType ct:
                    var charLit = print.Value.Eval() as CharLit;
                    result = charLit.Value.ToString() ?? "";
                    break;
                default:
                    throw new ArgumentException($"Print does not support type {print.Value.Type.TypeName}");
            }

            Console.WriteLine(result);
        }

        private IAST EvalRefTypeCreationStatement(RefTypeCreationStatement refType)
        {
            if (refType.CreatedReftype is Array)
                return CreateArray(refType.CreatedReftype as Array);
            if (refType.CreatedReftype is Struct)
                return CreateStruct(refType.CreatedReftype as Struct);

            throw new ArgumentException($"Unknown type for reference type creation: \"{refType.CreatedReftype.Type}\"");
        }

        private IAST CreateArray(Array array)
        {
            long size = (array.Size.Eval() as IntLit).Value ?? -1;

            if (size < 0)
                throw new ArgumentException("The length of an array has to be positive");

            array.Values = new IAST[size];

            return array;
        }

        private IAST CreateStruct(Struct @struct)
        {
            @struct.PropValues = new Dictionary<string, IAST>();
            StructDefinition structDef = Structs[@struct.Name];

            foreach (var prop in structDef.Properties)
                @struct.PropValues[prop.Name] = null;

            return @struct;
        }

        private IAST EvalIntArray(IntArray intArray)
        {
            if (intArray.Values == null)
                throw new ArgumentException("Array is not initialized");

            return intArray;
        }

        private IAST EvalDoubleArray(DoubleArray doubleArray)
        {
            if (doubleArray.Values == null)
                throw new ArgumentException("Array is not initialized");

            return doubleArray;
        }

        private IAST EvalBoolArray(BoolArray boolArray)
        {
            if (boolArray.Values == null)
                throw new ArgumentException("Array is not initialized");

            return boolArray;
        }

        private IAST EvalArrayIndexing(ArrayIndexing arrayIndexing)
        {
            Array array = arrayIndexing.Left.Eval() as Array;
            long index = (arrayIndexing.Right.Eval() as IntLit).Value ?? -1;
            long arraySize = (array.Size.Eval() as IntLit).Value ?? -1;

            if (index < 0)
                throw new ArgumentException("The index of an array must be initialized");

            if (index >= arraySize)
                throw new ArgumentException($"Index {index} out of range {(arraySize)}");

            return array.Values[index];
        }

        private void EvalAssignArrayField(AssignArrayField assignArrayField)
        {
            Array array = assignArrayField.ArrayIndex.Left.Eval() as Array;
            long index = (assignArrayField.ArrayIndex.Right.Eval() as IntLit).Value ?? -1;
            long arraySize = (array.Size.Eval() as IntLit).Value ?? -1;

            if (index < 0)
                throw new ArgumentException("The index of an array must be initialized");

            if (index >= arraySize)
                throw new ArgumentException($"Index {index} out of range {arraySize}");

            IAST value = assignArrayField.Value.Eval();

            array.Values[index] = value;
        }

        private IAST EvalStructPropertyAccess(StructPropertyAccess structPropertyAccess)
        {
            Struct @struct = structPropertyAccess.StructRef.Eval() as Struct;
            StructEnv = @struct.PropValues;
            var result = structPropertyAccess.Prop.Eval();

            StructEnv = null;

            return result;
        }

        private void EvalAssignStructProperty(AssignStructProperty assignStruct)
        {
            StructPropertyAccess innerStruct = GetPropRef(assignStruct.StructProp, out long index);
            Struct @struct = innerStruct.StructRef.Eval() as Struct;
            string propName = "";

            if (index >= 0)
                propName = ((innerStruct.Prop as ArrayIndexing).Left as VarExpr).Name;
            else
                propName = (innerStruct.Prop as VarExpr).Name;

            StructEnv = null;

            if (index >= 0)
                (@struct.PropValues[propName] as Array).Values[index] = assignStruct.Value.Eval();
            else
                @struct.PropValues[propName] = assignStruct.Value.Eval();
        }

        private IAST EvalValueAccessExpression(ValueAccessExpression valueAccess)
        {
            switch (valueAccess)
            {
                case VarExpr varExpr: return varExpr.Eval();
                case ArrayIndexing arrayIndexing: return arrayIndexing.Eval();
                case StructPropertyAccess structProperty: return structProperty.Eval();
                default:
                    throw new ArgumentException($"Unknown valueAccessExpression; On line {valueAccess.Line}:{valueAccess.Column}");
            }
        }

        private IAST EvalModExpr(ModExpr modExpr)
        {
            IntLit leftVal = modExpr.Left.Eval() as IntLit;
            IntLit rightVal = modExpr.Right.Eval() as IntLit;

            if (leftVal.Value == null)
                throw new ArgumentNullException($"Left operand is null; On line {modExpr.Line}:{modExpr.Column}");

            if (rightVal == null)
                throw new ArgumentException($"Right operand is null; On line {modExpr.Line}:{modExpr.Column}");

            return new IntLit((leftVal.Value ?? -1) % (rightVal.Value ?? -1), modExpr.Line, modExpr.Column);
        }

        private StructPropertyAccess GetPropRef(StructPropertyAccess val, out long index)
        {
            index = -1;
            if (val.Prop is VarExpr)
                return val;
            else if (val.Prop is ArrayIndexing arrayIndexing)
            {
                index = (arrayIndexing.Right.Eval() as IntLit).Value ?? -1;
                return val;
            }
            else
            {
                StructEnv = (val.StructRef.Eval() as Struct).PropValues;

                var result = GetPropRef(val.Prop as StructPropertyAccess, out long i);
                index = i;

                return result;
            }
        }
    }
}