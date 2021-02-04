using System.Collections.Generic;
using System;
using ll.type;

namespace ll.AST
{
    public class FunctionCall : IAST
    {
        public string name { get; set; }
        public List<IAST> args { get; set; }

        public FunctionCall(string name, List<IAST> args, int line, int column) : base(IAST.funs[name].returnType, line, column)
        {
            if (IAST.funs[name].args.Count != args.Count)
                throw new ArgumentException($"Argument count of functioncall \"{name}\" {args.Count} not equal to function definition {IAST.funs[name].args.Count}; On line {line}:{column}");
            var tmp = IAST.funs[name].args;
            for (int i = 0; i < tmp.Count; i++)
            {
                if (args[i].type != tmp[i].type)
                {
                    if(tmp[i].type is not DoubleType || args[i].type is not IntType)
                        throw new ArgumentException($"Type missmatch for \"{tmp[0]}\" \"{args[i].type.typeName}\" \"{tmp[i].type.typeName}\"; On line {line}:{column}");
                }
            }
            this.name = name;
            this.args = args;
        }
    }
}