using System;
using System.Collections.Generic;

namespace ll.AST
{
    public interface IAST
    {
        static Dictionary<string, double> env = new Dictionary<string, double>();
        double Eval()
        {
            switch (this)
            {
                case IntLit i: return i.n;
                case DoubleLit d: return d.n;
                case MultExpr me:
                    return me.left.Eval() * me.right.Eval();
                case AddExpr add:
                    return add.left.Eval() + add.right.Eval();
                case SubExpr sub:
                    return sub.left.Eval() - sub.right.Eval();
                case DivExpr div:
                    return ((double)div.left.Eval()) / div.right.Eval();
                case VarExpr varExpr:
                    return env[varExpr.name];
                case AssignStatement assign:
                    var tmp = assign.value.Eval();
                    env[assign.variable.name] = tmp;
                    return 0;
                case ExpressionSequenz sequenz:
                    int j;
                    for(j = 0; j < sequenz.body.Count - 1; j++)
                    {
                        sequenz.body[j].Eval();
                    }

                    if(j >= 0 && !(sequenz.body[j] is ReturnExpr))
                        throw new ArgumentException("last expression in expressionSequenz has to be an return Expression"); 

                    return sequenz.body[j].Eval();
                case ReturnExpr returnExpr:
                    return returnExpr.returnValue.Eval();
                case EqualityExpr equalityExpr:
                    if(equalityExpr.left.Eval() == equalityExpr.right.Eval())
                        return 1;
                    else
                        return 0;
                case LessExpr lessExpr:
                    if(lessExpr.left.Eval() < lessExpr.right.Eval())
                        return 1;
                    else
                        return 0;
                case GreaterExpr greaterExpr:
                    if(greaterExpr.left.Eval() > greaterExpr.right.Eval())
                        return 1;
                    else
                        return 0;
                default:
                    Console.WriteLine("Unknown Ast Object");
                    return 0;
            }
        }
    }
}