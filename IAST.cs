using System;
using System.Collections.Generic;

namespace ll
{
    public interface IAST
    {
        // TODO change from one environment to function based environments and one global
        static Dictionary<string, double> environment = new Dictionary<string, double>();
        double Eval()
        {
            switch (this)
            {
                case IntLit i: return i.n;
                case DoubleLit d: return d.n;
                case VarExpr v: return environment[v.name];
                case AssignExpr assignExpr:
                    double tmp = assignExpr.val.Eval();
                    if (!environment.ContainsKey(assignExpr.v.name))
                        environment.Add(assignExpr.v.name, tmp);
                    else
                        environment[assignExpr.v.name] = tmp;
                    return tmp;
                case MultExpr me:
                    return me.left.Eval() * me.right.Eval();
                case AddExpr add:
                    return add.left.Eval() + add.right.Eval();
                case SubExpr sub:
                    return sub.left.Eval() - sub.right.Eval();
                case DivExpr div:
                    return ((double)div.left.Eval()) / div.right.Eval();
                default:
                    Console.WriteLine("Unknown Ast Object");
                    return 0;
            }
        }
    }

    public class IntLit : IAST
    {
        public int n { get; set; }

        public IntLit(int n)
        {
            this.n = n;
        }
    }

    public class DoubleLit : IAST
    {
        public double n { get; set; }

        public DoubleLit(double n)
        {
            this.n = n;
        }
    }

    public class BinOp
    {
        public IAST left { get; set; }
        public IAST right { get; set; }
        public string op { get; set; }

        public BinOp(IAST left, IAST right, string op)
        {
            this.left = left;
            this.right = right;
            this.op = op;
        }
    }

    public class MultExpr : BinOp, IAST
    {
        public MultExpr(IAST left, IAST right) : base(left, right, "*")
        {

        }
    }

    public class AddExpr : BinOp, IAST
    {
        public AddExpr(IAST left, IAST right) : base(left, right, "+")
        {

        }
    }

    public class DivExpr : BinOp, IAST
    {
        public DivExpr(IAST left, IAST right) : base(left, right, "/")
        {

        }
    }

    public class SubExpr : BinOp, IAST
    {
        public SubExpr(IAST left, IAST right) : base(left, right, "-")
        {

        }
    }

    public class VarExpr : IAST
    {
        public string name { get; set; }

        public VarExpr(string name)
        {
            this.name = name;
        }
    }

    public class AssignExpr : IAST
    {
        public VarExpr v { get; set; }
        public IAST val { get; set; }

        public AssignExpr(VarExpr var, IAST val)
        {
            this.v = var;
            this.val = val;
        }
    }

    public class Sequenz : IAST
    {
        public List<IAST> body { get; set; }

        public Sequenz(List<IAST> body)
        {
            this.body = body;
        }
    }

    public class Function : IAST
    {
        public string name { get; set; }
        public List<string> args { get; set; }
        public IAST body { get; private set; }

        public Function(string name, List<string> args, IAST body)
        {
            this.name = name;
            this.args = args;
            this.body = body;
        }
    }

    public class FunctionCall : IAST
    {
        public string name { get; set; }
        public List<IAST> args { get; set; }
    }
}