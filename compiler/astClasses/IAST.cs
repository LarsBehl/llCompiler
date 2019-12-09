using System;
using System.Collections.Generic;
using ll.type;

namespace ll.AST
{
    public abstract class IAST
    {
        public static Dictionary<string, IAST> env;
        public static Dictionary<string, FunctionDefinition> funs = new Dictionary<string, FunctionDefinition>();
        public ll.type.Type type { get; set; }

        public IAST(ll.type.Type type)
        {
            this.type = type;
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
                        if(comp is ReturnStatement)
                        {
                            resultBlock = comp.Eval(); 
                            break;
                        }

                        if(comp is IfStatement && !(comp.type is IfStatementType))
                        {
                            resultBlock = comp.Eval();
                            if(resultBlock == null)
                                continue;
                            break;
                        }

                        if(comp is WhileStatement && !(comp.type is WhileStatementType))
                        {
                            resultBlock = comp.Eval();
                            break;
                        }

                        comp.Eval();
                    }

                    return resultBlock;
                case ReturnStatement returnExpr:
                    return returnExpr.returnValue.Eval();
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
                    if((ifStatement.cond.Eval() as BoolLit).value ?? false)
                        return ifStatement.ifBody.Eval();
                    return ifStatement.elseBody?.Eval() ?? null;
                case WhileStatement whileStatement:
                    IAST result_while = null;
                    while((whileStatement.condition.Eval() as BoolLit).value ?? false)
                    {
                        result_while = whileStatement.body.Eval();
                        if(result_while != null && !(result_while.type is BlockStatementType))
                            break;
                    }

                    return result_while;
                case IncrementExpr increment:
                    IAST result_inc;
                    if(increment.post)
                    {
                        result_inc = env[increment.variable.name];
                        env[increment.variable.name] = EvalIncrementExpr(increment);
                    }
                    else
                    {
                        result_inc = EvalIncrementExpr(increment);
                        env[increment.variable.name] = result_inc;
                    }

                    return result_inc;

                case DecrementExpr decrement:
                    IAST result_dec;
                    if(decrement.post)
                    {
                        result_dec = env[decrement.variable.name];
                        env[decrement.variable.name] = EvalDecrementExpr(decrement);
                    }
                    else
                    {
                        result_dec = EvalDecrementExpr(decrement);
                        env[decrement.variable.name] = result_dec;
                    }

                    return result_dec;
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
                        return new IntLit((me.left.Eval() as IntLit).n * (me.right.Eval() as IntLit).n);
                    if (me.right.type is DoubleType)
                        return new DoubleLit((me.left.Eval() as IntLit).n * (me.right.Eval() as DoubleLit).n);
                    throw new ArgumentException($"Type \"{me.left.type.typeName}\" is incompatible with \"{me.right.type.typeName}\"");
                case DoubleType d:
                    if (me.right.type is IntType)
                        return new DoubleLit((me.left.Eval() as DoubleLit).n * (me.right.Eval() as IntLit).n);
                    if (me.right.type is DoubleType)
                        return new DoubleLit((me.left.Eval() as DoubleLit).n * (me.right.Eval() as DoubleLit).n);
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
                        return new IntLit((div.left.Eval() as IntLit).n / (div.right.Eval() as IntLit).n);
                    if (div.right.type is DoubleType)
                        return new DoubleLit((div.left.Eval() as IntLit).n / (div.right.Eval() as DoubleLit).n);
                    throw new ArgumentException($"Type \"{div.left.type.typeName}\" is incompatible with \"{div.right.type.typeName}\"");
                case DoubleType d:
                    if (div.right.type is IntType)
                        return new DoubleLit((div.left.Eval() as DoubleLit).n / (div.right.Eval() as IntLit).n);
                    if (div.right.type is DoubleType)
                        return new DoubleLit((div.left.Eval() as DoubleLit).n / (div.right.Eval() as DoubleLit).n);
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
                        return new IntLit((add.left.Eval() as IntLit).n + (add.right.Eval() as IntLit).n);
                    if (add.right.type is DoubleType)
                        return new DoubleLit((add.left.Eval() as IntLit).n + (add.right.Eval() as DoubleLit).n);
                    throw new ArgumentException($"Type \"{add.left.type.typeName}\" is incompatible with \"{add.right.type.typeName}\"");
                case DoubleType d:
                    if (add.right.type is IntType)
                        return new DoubleLit((add.left.Eval() as DoubleLit).n + (add.right.Eval() as IntLit).n);
                    if (add.right.type is DoubleType)
                        return new DoubleLit((add.left.Eval() as DoubleLit).n + (add.right.Eval() as DoubleLit).n);
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
                        return new IntLit((sub.left.Eval() as IntLit).n - (sub.right.Eval() as IntLit).n);
                    if (sub.right.type is DoubleType)
                        return new DoubleLit((sub.left.Eval() as IntLit).n - (sub.right.Eval() as DoubleLit).n);
                    throw new ArgumentException($"Type \"{sub.left.type.typeName}\" is incompatible with \"{sub.right.type.typeName}\"");
                case DoubleType d:
                    if (sub.right.type is IntType)
                        return new DoubleLit((sub.left.Eval() as DoubleLit).n - (sub.right.Eval() as IntLit).n);
                    if (sub.right.type is DoubleType)
                        return new DoubleLit((sub.left.Eval() as DoubleLit).n - (sub.right.Eval() as DoubleLit).n);
                    throw new ArgumentException($"Type \"{sub.left.type.typeName}\" is incompatible with \"{sub.right.type.typeName}\"");
                default:
                    throw new ArgumentException($"Unknown type \"{sub.left.type.typeName}\"");
            }
        }

        static IAST EvalEqualityExpression(EqualityExpr eq)
        {
            switch (eq.left.type)
            {
                case IntType i:
                    if (eq.right.type is IntType)
                        return new BoolLit((eq.left.Eval() as IntLit).n == (eq.right.Eval() as IntLit).n);
                    if (eq.right.type is DoubleType)
                        return new BoolLit((eq.left.Eval() as IntLit).n == (eq.right.Eval() as DoubleLit).n);
                    throw new ArgumentException($"Type \"{eq.left.type.typeName}\" is incompatible with \"{eq.right.type.typeName}\"");
                case DoubleType d:
                    if (eq.right.type is IntType)
                        return new BoolLit((eq.left.Eval() as DoubleLit).n == (eq.right.Eval() as IntLit).n);
                    if (eq.right.type is DoubleType)
                        return new BoolLit((eq.left.Eval() as DoubleLit).n == (eq.right.Eval() as DoubleLit).n);
                    throw new ArgumentException($"Type \"{eq.left.type.typeName}\" is incompatible with \"{eq.right.type.typeName}\"");
                case BooleanType b:
                    if (!(eq.right.type is BooleanType))
                        throw new ArgumentException($"Type \"{eq.left.type.typeName}\" is incompatible with \"{eq.right.type.typeName}\"");
                    return new BoolLit((eq.left.Eval() as BoolLit).value == (eq.right.Eval() as BoolLit).value);
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
                            ? new BoolLit((le.left.Eval() as IntLit).n <= (le.right.Eval() as IntLit).n)
                            : new BoolLit((le.left.Eval() as IntLit).n < (le.right.Eval() as IntLit).n);
                        return result;
                    }

                    if (le.right.type is DoubleType)
                    {
                        IAST result = le.equal
                            ? new BoolLit((le.left.Eval() as IntLit).n <= (le.right.Eval() as DoubleLit).n)
                            : new BoolLit((le.left.Eval() as IntLit).n < (le.right.Eval() as DoubleLit).n);
                        return result;
                    }

                    throw new ArgumentException($"Type \"{le.left.type.typeName}\" is incompatible with \"{le.right.type.typeName}\"");
                case DoubleType d:
                    if (le.right.type is IntType)
                    {
                        IAST result = le.equal
                            ? new BoolLit((le.left.Eval() as DoubleLit).n <= (le.right.Eval() as IntLit).n)
                            : new BoolLit((le.left.Eval() as DoubleLit).n < (le.right.Eval() as IntLit).n);
                        return result;
                    }

                    if (le.right.type is DoubleType)
                    {
                        IAST result = le.equal
                            ? new BoolLit((le.left.Eval() as DoubleLit).n <= (le.right.Eval() as DoubleLit).n)
                            : new BoolLit((le.left.Eval() as DoubleLit).n < (le.right.Eval() as DoubleLit).n);
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
                            ? new BoolLit((ge.left.Eval() as IntLit).n >= (ge.right.Eval() as IntLit).n)
                            : new BoolLit((ge.left.Eval() as IntLit).n > (ge.right.Eval() as IntLit).n);
                        return result;
                    }

                    if (ge.right.type is DoubleType)
                    {
                        IAST result = ge.equal
                            ? new BoolLit((ge.left.Eval() as IntLit).n >= (ge.right.Eval() as DoubleLit).n)
                            : new BoolLit((ge.left.Eval() as IntLit).n > (ge.right.Eval() as DoubleLit).n);
                        return result;
                    }

                    throw new ArgumentException($"Type \"{ge.left.type.typeName}\" is incompatible with \"{ge.right.type.typeName}\"");
                case DoubleType d:
                    if (ge.right.type is IntType)
                    {
                        IAST result = ge.equal
                            ? new BoolLit((ge.left.Eval() as IntLit).n >= (ge.right.Eval() as IntLit).n)
                            : new BoolLit((ge.left.Eval() as IntLit).n > (ge.right.Eval() as IntLit).n);
                        return result;
                    }

                    if (ge.right.type is DoubleType)
                    {
                        IAST result = ge.equal
                            ? new BoolLit((ge.left.Eval() as IntLit).n >= (ge.right.Eval() as DoubleLit).n)
                            : new BoolLit((ge.left.Eval() as IntLit).n > (ge.right.Eval() as DoubleLit).n);
                        return result;
                    }

                    throw new ArgumentException($"Type \"{ge.left.type.typeName}\" is incompatible with \"{ge.right.type.typeName}\"");
                default:
                    throw new ArgumentException($"Unknown type \"{ge.left.type.typeName}\"");
            }
        }

        static IAST EvalIncrementExpr(IncrementExpr increment)
        {
            IAST variable;

            switch(increment.type)
            {
                case IntType intType:
                    variable = env[increment.variable.name].Eval();
                    return new IntLit((variable as IntLit).n + 1);
                case DoubleType doubleType:
                    variable = env[increment.variable.name].Eval();
                    return new DoubleLit((variable as DoubleLit).n +1);
                default:
                    throw new ArgumentException($"Type \"{increment.type.typeName}\" not allowed for Increment");
            }
        }

        static IAST EvalDecrementExpr(DecrementExpr decrement)
        {
            IAST variable;

            switch(decrement.type)
            {
                case IntType intType:
                    variable = env[decrement.variable.name].Eval();
                    return new IntLit((variable as IntLit).n - 1);
                case DoubleType doubleType:
                    variable = env[decrement.variable.name].Eval();
                    return new DoubleLit((variable as DoubleLit).n - 1);
                default:
                    throw new ArgumentException($"Type \"{decrement.type.typeName}\" not allowed for Decrement");
            }
        }

        static IAST EvalAddAssign(AddAssignStatement addAssign)
        {
            IAST variable;

            switch(addAssign.left.type)
            {
                case IntType intType:
                    variable = env[addAssign.left.name].Eval();
                    return new IntLit((variable as IntLit).n + (addAssign.right.Eval() as IntLit).n);
                case DoubleType doubleType:
                    variable = env[addAssign.left.name].Eval();
                    return new DoubleLit((variable as DoubleLit).n + (addAssign.right.Eval() as DoubleLit).n);
                default:
                    throw new ArgumentException($"Could not use type \"{addAssign.type.typeName}\" with AddAssignStatement");
            }
        }

        static IAST EvalSubAssign(SubAssignStatement subAssign)
        {
            IAST variable;

            switch(subAssign.left.type)
            {
                case IntType intType:
                    variable = env[subAssign.left.name].Eval();
                    return new IntLit((variable as IntLit).n - (subAssign.right.Eval() as IntLit).n);
                case DoubleType doubleType:
                    variable = env[subAssign.left.name].Eval();
                    return new DoubleLit((variable as DoubleLit).n - (subAssign.right.Eval() as DoubleLit).n);
                default:
                    throw new ArgumentException($"Could not use type \"{subAssign.type.typeName}\" with SubAssignStatement");
            }
        }

        static IAST EvalMultAssign(MultAssignStatement multAssign)
        {
            IAST variable;

            switch(multAssign.left.type)
            {
                case IntType intType:
                    variable = env[multAssign.left.name].Eval();
                    return new IntLit((variable as IntLit).n * (multAssign.right.Eval() as IntLit).n);
                case DoubleType doubleType:
                    variable = env[multAssign.left.name].Eval();
                    return new DoubleLit((variable as DoubleLit).n * (multAssign.right.Eval() as DoubleLit).n);
                default:
                    throw new ArgumentException($"Could not use type \"{multAssign.type.typeName}\" with MultAssignStatement");
            }
        }

        static IAST EvalDivAssign(DivAssignStatement divAssign)
        {
            IAST variable;

            switch(divAssign.left.type)
            {
                case IntType intType:
                    variable = env[divAssign.left.name].Eval();
                    return new IntLit((variable as IntLit).n / (divAssign.right.Eval() as IntLit).n);
                case DoubleType doubleType:
                    variable = env[divAssign.left.name].Eval();
                    return new DoubleLit((variable as DoubleLit).n / (divAssign.right.Eval() as DoubleLit).n);
                default:
                    throw new ArgumentException($"Could not use type \"{divAssign.type.typeName}\" with DivAssignStatement");
            }
        }
    }
}