using System;
using System.IO;
using System.Collections.Generic;

using Antlr4.Runtime;

using LL.AST;
using LL.CodeGeneration;
using LL.Exceptions;
using LL.Helper;

namespace LL
{
    class Program
    {
        static void InterpreterMode()
        {
            Console.WriteLine("Running in Interpreter Mode\n");
            string file = "InterpreterMode";
            ProgramNode rootProg = null;

            while (true)
            {
                Console.Write("> ");
                string text = Console.ReadLine();

                if (text == "?")
                {
                    Console.WriteLine("Available commands:");
                    Console.WriteLine("  \":fs\": Get all defined functions");
                    Console.WriteLine("  \":sts\": Get all defined structs");
                    continue;
                }

                if (text == ":fs")
                {
                    foreach (FunctionDefinition funDef in rootProg?.FunDefs.Values)
                        Console.WriteLine(funDef.Name);
                    continue;
                }

                if (text == ":sts")
                {
                    foreach (StructDefinition structDef in rootProg?.StructDefs.Values)
                        Console.WriteLine(structDef.Name);
                    continue;
                }

                if (string.IsNullOrEmpty(text))
                    break;

                rootProg = CompileContent(text, file, rootProg) as ProgramNode;

                if (rootProg is null)
                    continue;

                var value = rootProg.Eval();

                if (value is not null)
                    Console.WriteLine($"= {value.ToString()}");

                rootProg.CompositUnit = null;
                Console.WriteLine();
            }
        }

        static void InteractiveCompilerMode()
        {
            string file = "InteractiveCompilerMode";
            
            Console.WriteLine("Running in interactive Compilermode\n");
            while (true)
            {
                Console.Write("> ");
                string text = Console.ReadLine();

                if (string.IsNullOrEmpty(text))
                    break;


                ProgramNode ast = CompilationHelper.CompileContent(text);

                // prevent nullpointer
                if (ast is null)
                    continue;

                var assemblerGenerator = new AssemblerGenerator(file);
                assemblerGenerator.GenerateAssember(ast);
                assemblerGenerator.PrintAssember();
            }
        }

        static void CompilerMode(string inputFile)
        {
            Console.WriteLine($"Compiling {inputFile}...");

            ProgramNode ast = CompilationHelper.CompileFile(inputFile);
            // prevent nullpointer
            if (ast is null)
                Environment.Exit(-1);

            var assemblerGenerator = new AssemblerGenerator(inputFile);
            assemblerGenerator.WriteToFile(inputFile, ast);
        }

        private static IAST CompileContent(string content, string fileName, ProgramNode rootProgram)
        {
            IAST ast = null;
            try
            {
                // setup the needed environment
                llParser parser = new llParser(new CommonTokenStream(new llLexer(new AntlrInputStream(content))));
                parser.RemoveErrorListeners();
                parser.AddErrorListener(new ErrorListener(fileName));

                // parse the struct definitions and the load statements
                ProgramNode prog = new StructDefinitionVisitor(fileName, rootProgram).VisitCompileUnit(parser.compileUnit()) as ProgramNode;

                // parse the function definitions
                parser.Reset();
                prog = new FunctionDefinitionVisitor(fileName, prog).VisitCompileUnit(parser.compileUnit()) as ProgramNode;

                // parse the complete program
                parser.Reset();
                ast = new BuildAstVisitor(prog).VisitCompileUnit(parser.compileUnit());
            }
            catch (BaseCompilerException e)
            {
                CompilationHelper.PrintError(e);
                Console.WriteLine();

                return rootProgram;
            }

            return ast;
        }

        static void RTFM()
        {
            Console.WriteLine("Usage: ./llCompiler <flag> [file]");
            Console.WriteLine("  Flags:");
            Console.WriteLine("    \"-i\": Run compiler in interpreter mode");
            Console.WriteLine("    \"-c\": Run compiler in compiler mode; In this mode [file] must be specified");
        }

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0] == "-i")
                {
                    Program.InterpreterMode();
                    return;
                }
                if (args[0] == "-c" && args.Length == 2)
                    Program.CompilerMode(args[1]);
                else
                {
                    Console.WriteLine($"unknown flag {args[0]}");
                    Program.RTFM();
                }
            }
            else
            {
# if DEBUG
                Program.InteractiveCompilerMode();
# elif RELEASE
                Program.RTFM();
#endif
            }
        }
    }
}
