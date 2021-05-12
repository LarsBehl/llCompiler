using System;
using System.Collections.Generic;

using LL.Types;

namespace LL.AST
{
    public class ProgramNode : IAST
    {
        public Dictionary<string, FunctionDefinition> FunDefs { get; set; }
        public Dictionary<string, StructDefinition> StructDefs { get; set; }
        public Dictionary<string, LoadStatement> Dependencies { get; set; }
        public IAST CompositUnit { get; set; }
        public Dictionary<string, IAST> Env { get; set; }
        public string FileName { get; set; }

        public ProgramNode(
            string fileName,
            int line, int column
        ) : this(
            fileName,
            new Dictionary<string, FunctionDefinition>(),
            new Dictionary<string, StructDefinition>(),
            line,
            column
        )
        {

        }

        public ProgramNode(
            string fileName,
            Dictionary<string, FunctionDefinition> funDefs,
            Dictionary<string, StructDefinition> structDefs,
            int line,
            int column
        ) : base(
            new ProgramType(),
            line,
            column
        )
        {
            this.FunDefs = funDefs;
            this.StructDefs = structDefs;
            this.Dependencies = new Dictionary<string, LoadStatement>();
            this.CompositUnit = null;
            this.Env = new Dictionary<string, IAST>();
            this.FileName = fileName;
        }

        public bool TryAddStructDefinition(StructDefinition structDef)
        {
            bool result = false;

            if(!this.ContainsStruct(structDef.Name))
            {
                this.StructDefs[structDef.Name] = structDef;
                result = true;
            }
            
            return result;
        }

        public bool ContainsStruct(string structName)
        {
            bool result = this.StructDefs.ContainsKey(structName);

            if(result)
                return result;
            
            foreach(LoadStatement dep in this.Dependencies.Values)
            {
                result = dep.Program.ContainsStruct(structName);

                if(result)
                    return result;
            }

            return false;
        }

        public bool ContainsFunction(string functionName)
        {
            bool result = this.FunDefs.ContainsKey(functionName);

            if(result)
                return result;
            
            foreach(LoadStatement dep in this.Dependencies.Values)
            {
                result = dep.Program.ContainsFunction(functionName);

                if(result)
                    return result;
            }

            return false;
        }
    }
}