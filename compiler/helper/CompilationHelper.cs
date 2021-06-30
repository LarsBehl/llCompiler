using System;
using System.Linq;
using System.Collections.Generic;

using Antlr4.Runtime;

using LL.AST;
using LL.Exceptions;

namespace LL.Helper
{
    public class CompilationHelper
    {
        public static ProgramNode CompileFile(string fileLocation)
        {
            llParser parser = null;

            try
            {
                parser = new llParser(new CommonTokenStream(new llLexer(new AntlrFileStream(fileLocation))));
            }
            catch (System.IO.FileNotFoundException)
            {
                var notFoundException = new LL.Exceptions.FileNotFoundException(fileLocation, null, 0, 0);
                CompilationHelper.PrintError(notFoundException);
                Environment.Exit(-1);
            }

            return CompileContent(parser, fileLocation);
        }

        public static ProgramNode ParseStructAndLoad(string fileLocation)
        {
            llParser parser = null;

            try
            {
                parser = new llParser(new CommonTokenStream(new llLexer(new AntlrFileStream(fileLocation))));
            }
            catch (System.IO.FileNotFoundException)
            {
                var notFoundException = new LL.Exceptions.FileNotFoundException(fileLocation, null, 0, 0);
                CompilationHelper.PrintError(notFoundException);
                Environment.Exit(-1);
            }

            return ParseStructAndLoad(parser, fileLocation);
        }

        private static ProgramNode ParseStructAndLoad(llParser parser, string fileLocation)
        {
            ProgramNode result = null;
            parser.RemoveErrorListeners();
            parser.AddErrorListener(new ErrorListener(fileLocation));

            try
            {
                result = new StructDefinitionVisitor(fileLocation).VisitCompileUnit(parser.compileUnit()) as ProgramNode;
                parser.Reset();
            }
            catch (BaseCompilerException e)
            {
                PrintError(e);
                Environment.Exit(-1);
            }

            result.Parser = parser;

            return result;
        }

        public static ProgramNode CompileContent(string content)
        {
            llParser parser = new llParser(new CommonTokenStream(new llLexer(new AntlrInputStream(content))));

            return CompileContent(parser, "");
        }

        private static ProgramNode CompileContent(llParser parser, string fileLocation)
        {
            ProgramNode result = null;
            parser.RemoveErrorListeners();
            parser.AddErrorListener(new ErrorListener(fileLocation));

            try
            {
                result = new StructDefinitionVisitor(fileLocation).VisitCompileUnit(parser.compileUnit()) as ProgramNode;
                StructDefinitionVisitor.ProgData.RootProgram = result;
                parser.Reset();
                result.Parser = parser;

                ConflictingImportedGlobalVariables(result);
                ConflictingImportedStructs(result);

                List<ProgramNode> nodes = StructDefinitionVisitor.ProgData.ContainsCircularDependency();

                if (nodes is null || nodes.Count <= 0)
                    throw new UnexpectedErrorException(fileLocation);

                foreach (ProgramNode node in nodes)
                {
                    new FunctionDefinitionVisitor(node).VisitCompileUnit(node.Parser.compileUnit());
                    node.Parser.Reset();


                    new BuildAstVisitor(node).VisitCompileUnit(node.Parser.compileUnit());
                }

                ConflictingImportedFunctions(result);
            }
            catch (BaseCompilerException e)
            {
                PrintError(e);
                Environment.Exit(-1);
            }

            return result;
        }

        public static Dictionary<string, string> ConflictingImportedFunctions(ProgramNode prog)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            List<Dictionary<string, string>> functionsInChildren = new List<Dictionary<string, string>>();

            // get the struct definitions of the children
            foreach (var dep in prog.Dependencies?.Values)
                functionsInChildren.Add(ConflictingImportedFunctions(dep.Program));

            for (int i = 0; i < functionsInChildren.Count; i++)
            {
                Dictionary<string, string> child1Functions = functionsInChildren[i];
                for (int j = i + 1; j < functionsInChildren.Count; j++)
                {
                    Dictionary<string, string> child2Funtions = functionsInChildren[j];
                    // check if there are any conflicts
                    foreach (string functionName in child1Functions.Keys)
                    {
                        bool success = child2Funtions.TryGetValue(functionName, out string fileName);

                        if (success && child1Functions[functionName] != fileName)
                            throw new ConflictingImportException(
                                functionName,
                                child1Functions[functionName],
                                fileName,
                                "function",
                                prog.FileName,
                                -1,
                                -1
                            );
                    }
                }

                foreach (var function in child1Functions)
                    result[function.Key] = function.Value;
            }

            foreach (var function in prog.FunDefs)
                result.Add(function.Key, prog.FileName);

            return result;
        }

        public static Dictionary<string, string> ConflictingImportedStructs(ProgramNode prog)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            List<Dictionary<string, string>> structsInChildren = new List<Dictionary<string, string>>();

            // get the struct definitions of the children
            foreach (var dep in prog.Dependencies?.Values)
                structsInChildren.Add(ConflictingImportedStructs(dep.Program));

            for (int i = 0; i < structsInChildren.Count; i++)
            {
                Dictionary<string, string> child1Structs = structsInChildren[i];
                for (int j = i + 1; j < structsInChildren.Count; j++)
                {
                    Dictionary<string, string> child2Structs = structsInChildren[j];
                    // check if there are any conflicts
                    foreach (string structName in child1Structs.Keys)
                    {
                        bool success = child2Structs.TryGetValue(structName, out string fileName);

                        if (success && child1Structs[structName] != fileName)
                            throw new ConflictingImportException(
                                structName,
                                child1Structs[structName],
                                fileName,
                                "struct",
                                prog.FileName,
                                -1,
                                -1
                            );
                    }
                }

                foreach (var @struct in child1Structs)
                    result[@struct.Key] = @struct.Value;
            }

            foreach (var @struct in prog.StructDefs)
                result.Add(@struct.Key, prog.FileName);

            return result;
        }

        public static Dictionary<string, string> ConflictingImportedGlobalVariables(ProgramNode prog)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            List<Dictionary<string, string>> globalVariablesInChildren = new List<Dictionary<string, string>>();

            // get the struct definitions of the children
            foreach (var dep in prog.Dependencies?.Values)
                globalVariablesInChildren.Add(ConflictingImportedGlobalVariables(dep.Program));

            for (int i = 0; i < globalVariablesInChildren.Count; i++)
            {
                Dictionary<string, string> child1GlobalVariables = globalVariablesInChildren[i];
                for (int j = i + 1; j < globalVariablesInChildren.Count; j++)
                {
                    Dictionary<string, string> child2globalVariables = globalVariablesInChildren[j];
                    // check if there are any conflicts
                    foreach (string structName in child1GlobalVariables.Keys)
                    {
                        bool success = child2globalVariables.TryGetValue(structName, out string fileName);

                        if (success && child1GlobalVariables[structName] != fileName)
                            throw new ConflictingImportException(
                                structName,
                                child1GlobalVariables[structName],
                                fileName,
                                "globalVariable",
                                prog.FileName,
                                -1,
                                -1
                            );
                    }
                }

                foreach (var globalVariable in child1GlobalVariables)
                    result[globalVariable.Key] = globalVariable.Value;
            }

            foreach (var globalVariable in prog.GlobalVariables)
                result.Add(globalVariable.Key, prog.FileName);

            return result;
        }

        public static void PrintError(BaseCompilerException e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}