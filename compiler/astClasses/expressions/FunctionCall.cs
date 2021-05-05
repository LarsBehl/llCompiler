using System.Collections.Generic;
using System;
using LL.Types;

namespace LL.AST
{
    public class FunctionCall : IAST
    {
        public string FunctionName { get; set; }
        public List<IAST> Args { get; set; }

        // TODO add exception handling
        public FunctionCall(FunctionDefinition funDef, string name, List<IAST> args, int line, int column) : base(funDef.ReturnType, line, column)
        {
            if (funDef.Args.Count != args.Count)
                throw new ArgumentException($"Argument count of functioncall \"{name}\" {args.Count} not equal to function definition {funDef.Args.Count}; On line {line}:{column}");
            var tmp = funDef.Args;
            for (int i = 0; i < tmp.Count; i++)
            {
                if (args[i].Type != tmp[i].Type)
                {
                    if(tmp[i].Type is not DoubleType || args[i].Type is not IntType)
                        throw new ArgumentException($"Type missmatch for \"{tmp[0]}\" \"{args[i].Type.TypeName}\" \"{tmp[i].Type.TypeName}\"; On line {line}:{column}");
                }
            }
            this.FunctionName = name;
            this.Args = args;
        }
    }
}