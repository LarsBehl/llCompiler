using System;
using System.Collections.Generic;

namespace ll
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
                case AssignExpr assign:
                    var tmp = assign.value.Eval();
                    env[assign.variable.name] = tmp;
                    return tmp;
                case ExpressionSequenz sequenz:
                    int j;
                    for(j = 0; j < sequenz.body.Count - 1; j++)
                    {
                        sequenz.body[j].Eval();
                    }

                    return sequenz.body[j].Eval();
                default:
                    Console.WriteLine("Unknown Ast Object");
                    return 0;
            }
        }
    }
}