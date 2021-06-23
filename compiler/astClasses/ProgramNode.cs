using System.Collections.Generic;

using LL.Exceptions;
using LL.Helper;
using LL.Types;

namespace LL.AST
{
    public class ProgramNode : IAST
    {
        public Dictionary<string, FunctionDefinition> FunDefs { get; set; }
        public Dictionary<string, StructDefinition> StructDefs { get; set; }
        public Dictionary<string, LoadStatement> Dependencies { get; set; }
        public Dictionary<string, GlobalVariableStatement> GlobalVariables { get; set; }
        public IAST CompositUnit { get; set; }
        public Dictionary<string, IAST> Env { get; set; }
        public string FileName { get; set; }
        public bool IsHeader { get; set; }
        public llParser Parser { get; set; }

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
            this.GlobalVariables = new Dictionary<string, GlobalVariableStatement>();
            this.CompositUnit = null;
            this.Env = new Dictionary<string, IAST>();
            this.FileName = fileName;
            this.IsHeader = this.FileName.EndsWith(Constants.HEADER_FILE_ENDING);
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

        public bool IsGlobalVariableDefined(string globalVarName)
        {
            bool result = this.GlobalVariables.ContainsKey(globalVarName);

            if (result)
                return result;

            foreach (LoadStatement dep in this.Dependencies.Values)
            {
                result = dep.Program.IsGlobalVariableDefined(globalVarName);

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

            if (success)
                return result;

            foreach (LoadStatement dep in this.Dependencies.Values)
            {
                success = dep.Program.FunDefs.TryGetValue(functionName, out result);

                if (success)
                    return result;
            }

            throw new UnexpectedErrorException(FileName);
        }

        public StructDefinition GetStructDefinition(string structName)
        {
            bool success = this.StructDefs.TryGetValue(structName, out StructDefinition result);

            if (success)
                return result;

            foreach (LoadStatement dep in this.Dependencies.Values)
            {
                result = dep.Program.GetStructDefinition(structName);

                if (result is not null)
                    return result;
            }

            return null;
        }

        public GlobalVariableStatement GetGlobalVariableStatement(string variableName)
        {
            bool success = this.GlobalVariables.TryGetValue(variableName, out GlobalVariableStatement result);

            if (success)
                return result;

            foreach (LoadStatement dep in this.Dependencies.Values)
            {
                result = dep.Program.GetGlobalVariableStatement(variableName);

                if (result is not null)
                    return result;
            }

            return null;
        }

        public static bool operator ==(ProgramNode n1, ProgramNode n2)
        {
            if (n1 is null && n2 is null)
                return true;

            if (n1 is not null && n2 is not null)
                return n1.FileName == n2.FileName;

            return false;
        }

        public static bool operator !=(ProgramNode n1, ProgramNode n2)
        {
            if (n1 is null && n2 is null)
                return false;

            if (n1 is null || n2 is null)
                return true;

            return n1.FileName != n2.FileName;
        }

        public override bool Equals(object obj)
        {
            if (obj is ProgramNode pn)
            {
                return pn == this;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}