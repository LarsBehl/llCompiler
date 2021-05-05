using System;
using System.IO;
using System.Collections.Generic;
using Antlr4.Runtime;
using LL.AST;
using LL.CodeGeneration;
using LL.Exceptions;

namespace LL
{
    class Program
    {
        static void InterpreterMode()
        {
            Console.WriteLine("Running in Interpreter Mode\n");
            string file = "InterpreterMode";
            Dictionary<string, FunctionDefinition> funs = new Dictionary<string, FunctionDefinition>();
            Dictionary<string, StructDefinition> structs = new Dictionary<string, StructDefinition>();

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
                    foreach (FunctionDefinition funDef in funs.Values)
                        Console.WriteLine(funDef.Name);
                    continue;
                }

                if (text == ":sts")
                {
                    foreach (StructDefinition structDef in structs.Values)
                        Console.WriteLine(structDef.Name);
                    continue;
                }

                if (string.IsNullOrEmpty(text))
                    break;

                IAST ast = CompileContent(text, file);

                if(ast is null)
                    continue;

                // if it is a programNode all defined functions and structs need to be stored
                if(ast is ProgramNode pn)
                {
                    bool doContinue = false;
                    foreach(var structDef in pn.StructDefs)
                    {
                        if(structs.ContainsKey(structDef.Key))
                        {
                            PrintError(new StructAlreadyDefinedException(structDef.Key, file, structDef.Value.Line, structDef.Value.Column));
                            doContinue = true;
                            break;
                        }

                        structs.Add(structDef.Key, structDef.Value);
                    }

                    foreach(var funDef in pn.FunDefs)
                    {
                        if(funs.ContainsKey(funDef.Key))
                        {
                            PrintError(new FunctionAlreadyDefinedException(funDef.Key, file, funDef.Value.Line, funDef.Value.Column));
                            doContinue = true;
                            break;
                        }

                        funs.Add(funDef.Key, funDef.Value);
                    }

                    if(doContinue)
                        continue;
                    
                }
                else
                {
                    IAST.Funs = funs;
                    IAST.Structs = structs;
                }

                var value = ast.Eval();

                if(value is not null)
                    Console.WriteLine($"= {value.ToString()}");
                
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


                IAST ast = CompileContent(text, file);

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

            string content = null;
            try
            {
                using (StreamReader reader = new StreamReader(new FileStream(inputFile, FileMode.Open)))
                {
                    content = reader.ReadToEnd();
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                var notFoundException = new LL.Exceptions.FileNotFoundException(inputFile, null, 0, 0);
                PrintError(notFoundException);
                Environment.Exit(-1);
            }

            IAST ast = CompileContent(content, inputFile);
            // prevent nullpointer
            if (ast is null)
                Environment.Exit(-1);

            var assemblerGenerator = new AssemblerGenerator(inputFile);
            assemblerGenerator.WriteToFile(inputFile, ast);
        }

        private static IAST CompileContent(string content, string fileName)
        {
            IAST ast = null;
            try
            {
                // setup the needed environment
                llParser parser = new llParser(new CommonTokenStream(new llLexer(new AntlrInputStream(content))));
                parser.RemoveErrorListeners();
                parser.AddErrorListener(new ErrorListener());

                // parse the struct definitions and the load statements
                ProgramNode prog = new StructDefinitionVisitor(fileName).VisitCompileUnit(parser.compileUnit()) as ProgramNode;

                // parse the function definitions
                parser.Reset();
                prog = new FunctionDefinitionVisitor(fileName, prog).VisitCompileUnit(parser.compileUnit()) as ProgramNode;

                // parse the complete program
                parser.Reset();
                ast = new BuildAstVisitor(fileName, prog).VisitCompileUnit(parser.compileUnit());
            }
            catch (BaseCompilerException e)
            {
                PrintError(e);
                Console.WriteLine();
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

        private static void PrintError(BaseCompilerException e)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(e.Message);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
