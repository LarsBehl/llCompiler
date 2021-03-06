using System.Collections.Generic;
using LL.Types;
using System;

namespace LL.AST
{
    public class FunctionDefinition : IAST
    {
        public string Name { get; set; }
        public List<InstantiationStatement> Args { get; set; }
        public IAST Body { get; set; }
        public Dictionary<string, IAST> FunctionEnv { get; set; }
        public Types.Type ReturnType { get; set; }

        public FunctionDefinition(
            string name,
            List<InstantiationStatement> args,
            Dictionary<string, IAST> functionEnv,
            Types.Type returnType,
            int line,
            int column
        ) : base(new FunctionDefinitionType(), line, column)
        {
            this.Name = name;
            this.Args = args;
            this.Body = null;
            this.FunctionEnv = functionEnv;
            this.ReturnType = returnType;

            if(this.ReturnType is not VoidType && this.Name == "main")
                throw new ArgumentException($"Main method has to be void; On line {line}:{column}");
        }

        public int GetLocalVariables()
        {
            return FunctionEnv.Count - Args.Count;
        }

        /// <summary>
        /// Checks whether or not the function definition is only a prototype
        /// </summary>
        /// <return>True if the definition is a prototype, false otherwise</return>
        public bool isPrototype()
        {
            return this.Body is null;
        }
    }
}