using System.Collections.Generic;
using System;

namespace ll.AST
{
    public class FunctionCall : IAST
    {
        public string name { get; set; }
        public List<IAST> args { get; set; }

        public FunctionCall(string name, List<IAST> args) : base(IAST.funs[name].returnType)
        {
            if(IAST.funs[name].args.Count != args.Count)
                throw new ArgumentException($"Argument count of functioncall \"{name}\" {args.Count} not equal to function definition {IAST.funs[name].args.Count}");
            var tmp = IAST.funs[name].args;
            for(int i = 1; i < tmp.Count; i++)
            {
                if(args[i].type.typeName != tmp[i].type.typeName)
                    throw new ArgumentException($"Type missmatch for \"{tmp[0]}\" \"{args[i].type.typeName}\" \"{tmp[i].type.typeName}\"");
            }
            this.name = name;
            this.args = args;
        }
    }
}