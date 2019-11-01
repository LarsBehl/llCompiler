using System;
using System.Collections.Generic;

namespace ll
{
    public interface IAST
    {
        double Eval()
        {
            return this.Eval(new List<Function>());
        }

        double Eval(List<Function> functions)
        {
            Dictionary<string, Function> fs = new Dictionary<string, Function>();
            foreach(Function f in functions)
            {
                fs.TryAdd(f.name, f);
            }

            return this.Eval(fs, new Dictionary<string, double>());
        }

        double Eval(Dictionary<string, Function> functions, Dictionary<string, double> env)
        {
            switch (this)
            {
                case IntLit i: return i.n;
                case DoubleLit d: return d.n;
                case VarExpr v: return env[v.name];
                case AssignExpr assignExpr:
                    double tmp = assignExpr.val.Eval();
                    if (!env.ContainsKey(assignExpr.v.name))
                        env.Add(assignExpr.v.name, tmp);
                    else
                        env[assignExpr.v.name] = tmp;
                    return tmp;
                case MultExpr me:
                    return me.left.Eval() * me.right.Eval();
                case AddExpr add:
                    return add.left.Eval() + add.right.Eval();
                case SubExpr sub:
                    return sub.left.Eval() - sub.right.Eval();
                case DivExpr div:
                    return ((double)div.left.Eval()) / div.right.Eval();
                case Sequenz sequenz:
                    int j;
                    for(j = 0; j < sequenz.body.Count-1; j++)
                    {
                        sequenz.body[j].Eval();
                    }
                    if(!(sequenz.body[j] is VarExpr))
                        throw new ArgumentException("Last expression in sequenz must be a variable (return value)");
                    return sequenz.body[j].Eval();
                default:
                    Console.WriteLine("Unknown Ast Object");
                    return 0;
            }
        }
    }
}