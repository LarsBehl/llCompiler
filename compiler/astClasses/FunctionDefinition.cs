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
        public type.Type returnType { get; set; }

        public FunctionDefinition(
            string name,
            List<InstantiationStatement> args,
            IAST body,
            Dictionary<string, IAST> functionEnv,
            type.Type returnType
        ) : base(new FunctionDefinitionType())
        {
            this.name = name;
            this.args = args;
            this.body = body;
            this.functionEnv = functionEnv;
            this.returnType = returnType;
        }
    }
}