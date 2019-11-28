using System.Collections.Generic;
using ll.type;
using System;

namespace ll.AST
{
    public class FunctionDefinition : IAST
    {
        public string name { get; set; }
        public List<InstantiationStatement> args { get; set; }
        public IAST body { get; set; }
        public Dictionary<string, IAST> functionEnv { get; set; }

        public FunctionDefinition(
            string name,
            List<InstantiationStatement> args, 
            IAST body, 
            Dictionary<string, IAST> functionEnv, 
            type.Type type
        ) : base(type)
        {
            if(body.type.typeName != type.typeName)
                throw new ArgumentException($"Return type \"{body.type.typeName}\" does not match \"{type.typeName}\"");

            this.name = name;
            this.args = args;
            this.body = body;
            this.functionEnv = functionEnv;
        }
    }
}