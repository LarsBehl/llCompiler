using System;
using System.Collections.Generic;
using ll.type;

namespace ll.AST
{
    public abstract class IAST
    {
        public static Dictionary<string, IAST> env = new Dictionary<string, IAST>();
        public static Dictionary<string, ll.type.Type> typeDefs = new Dictionary<string, ll.type.Type>();
        public ll.type.Type type { get; set; }

        public IAST(ll.type.Type type)
        {
            this.type = type;
        }

        public static ll.type.Type GetType(string varName)
        {
            return typeDefs[varName];
        }

        public static void SetType(string varName, ll.type.Type type)
        {
            if(typeDefs.ContainsKey(varName) && typeDefs[varName] != type)
                throw new ArgumentException($"There is already a variable \"{varName}\" with type \"{typeDefs[varName].typeName}\"");
            typeDefs[varName] = type;
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
                    return env[varExpr.name];
                case AssignStatement assign:
                    var tmp = assign.value.Eval();
                    env[assign.variable.name] = tmp;
                    return null;
                case ExpressionSequenz sequenz:
                    int j;
                    for (j = 0; j < sequenz.body.Count - 1; j++)
                    {
                        sequenz.body[j].Eval();
                    }

                    if (j >= 0 && !(sequenz.body[j] is ReturnStatement))
                        throw new ArgumentException("last expression in expressionSequenz has to be an return Expression");

                    return sequenz.body[j].Eval();
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
                default:
                    Console.WriteLine("Unknown Ast Object");
                    return null;
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
    }
}