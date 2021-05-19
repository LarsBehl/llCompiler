using System;
using System.Text;
using System.Collections.Generic;

using LL.AST;
using LL.Exceptions;
using LL.Types;

namespace LL.CodeGeneration
{
    public class HeaderGenerator
    {
        private StringBuilder StructBuilder;
        private StringBuilder FunctionPrototypeBuilder;
        private StringBuilder LoadStatementBuilder;
        private ProgramNode RootProg;
        private static List<string> CreatedHeaders = new List<string>();
        private static string Indentation = "    ";

        public HeaderGenerator(ProgramNode rootProg)
        {
            this.RootProg = rootProg;
            this.StructBuilder = new StringBuilder();
            this.FunctionPrototypeBuilder = new StringBuilder();
            this.LoadStatementBuilder = new StringBuilder();
        }

        /// <summary>
        /// Creates the header files for the given program
        /// </summary>
        public void CreateHeader()
        {
            CreatedHeaders.Add(this.RootProg.FileName);

            foreach (LoadStatement loadStatement in this.RootProg.Dependencies.Values)
                this.AppendLoadStatement(loadStatement);
            foreach (StructDefinition structDefinition in this.RootProg.StructDefs.Values)
                this.AppendStructDefinition(structDefinition);
            foreach (FunctionDefinition functionDefinition in this.RootProg.FunDefs.Values)
                this.AppendFunctionPrototype(functionDefinition);

            // TODO write the header file
            Console.WriteLine(this.LoadStatementBuilder.ToString());
            Console.WriteLine(this.StructBuilder.ToString() + this.FunctionPrototypeBuilder.ToString());
        }

        private void AppendLoadStatement(LoadStatement loadStatement)
        {
            this.LoadStatementBuilder.AppendLine($"load {loadStatement.FileName}");
        }

        private void AppendStructDefinition(StructDefinition structDefinition)
        {
            this.StructBuilder.AppendLine($"struct {structDefinition.Name}");
            this.StructBuilder.AppendLine("{");

            foreach (StructProperty property in structDefinition.Properties)
                this.StructBuilder.AppendLine($"{Indentation}{property.Name}:{this.GetTypeName(property.Type)};");

            this.StructBuilder.AppendLine("}");
            this.StructBuilder.AppendLine();
        }

        private void AppendFunctionPrototype(FunctionDefinition functionDefinition)
        {
            this.FunctionPrototypeBuilder.Append($"{functionDefinition.Name}(");

            bool first = true;
            foreach (InstantiationStatement arg in functionDefinition.Args)
            {
                if (first)
                    first = false;
                else
                    this.FunctionPrototypeBuilder.Append(", ");

                this.FunctionPrototypeBuilder.Append($"{arg.Name}:{this.GetTypeName(arg.Type)}");
            }
            this.FunctionPrototypeBuilder.AppendLine($"):{this.GetTypeName(functionDefinition.ReturnType)};");
        }

        private string GetTypeName(LL.Types.Type type)
        {
            string result = "";

            switch (type)
            {
                case StructType structType:
                    result = structType.StructName;
                    break;
                default:
                    result = type.TypeName;
                    break;
            }

            return result;
        }
    }
}