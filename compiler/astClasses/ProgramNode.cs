using System.Collections.Generic;

using LL.Exceptions;
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

            if (!this.ContainsStruct(structDef.Name))
            {
                this.StructDefs[structDef.Name] = structDef;
                result = true;
            }

            return result;
        }

        public bool ContainsStruct(string structName)
        {
            bool result = this.StructDefs.ContainsKey(structName);

            if (result)
                return result;

            foreach (LoadStatement dep in this.Dependencies.Values)
            {
                result = dep.Program.ContainsStruct(structName);

                if (result)
                    return result;
            }

            return false;
        }

        public bool IsFunctionDefined(string functionName)
        {
            bool result = this.FunDefs.ContainsKey(functionName);

            if (result)
                return result;

            foreach (LoadStatement dep in this.Dependencies.Values)
            {
                result = dep.Program.IsFunctionDefined(functionName);

                if (result)
                    return result;
            }

            return false;
        }

        public bool IsFunctionCallable(string functionName)
        {
            bool result = this.FunDefs.ContainsKey(functionName);

            if (result)
                return result;

            foreach (LoadStatement dep in this.Dependencies.Values)
            {
                result = dep.Program.FunDefs.ContainsKey(functionName);

                if (result)
                    return result;
            }

            return false;
        }

        public FunctionDefinition GetFunctionDefinition(string functionName)
        {
            bool success = this.FunDefs.TryGetValue(functionName, out FunctionDefinition result);

            if(success)
                return result;
            
            foreach(LoadStatement dep in this.Dependencies.Values)
            {
                success = dep.Program.FunDefs.TryGetValue(functionName, out result);

                if(success)
                    return result;
            }
        
            throw new UnexpectedErrorException(FileName);
        }

        public StructDefinition GetStructDefinition(string structName)
        {
            bool success = this.StructDefs.TryGetValue(structName, out StructDefinition result);

            if(success)
                return result;
            
            foreach(LoadStatement dep in this.Dependencies.Values)
            {
                result = dep.Program.GetStructDefinition(structName);

                if(result is not null)
                    return result;
            }

            return null;
        }
    }
}