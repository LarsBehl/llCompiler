using System;
using System.IO;
using Antlr4.Runtime;
using LL.AST;
using LL.CodeGeneration;

namespace LL
{
    class Program
    {
        static void InterpreterMode()
        {
            Console.WriteLine("Running in Interpreter Mode\n");
            string file = "InterpreterMode";

            while (true)
            {
                Console.Write("> ");
                string text = Console.ReadLine();

                if(text == "?")
                {
                    Console.WriteLine("Available commands:");
                    Console.WriteLine("  \":fs\": Get all defined functions");
                    Console.WriteLine("  \":sts\": Get all defined structs");
                    continue;
                }

                if (text == ":fs")
                {
                    foreach (FunctionDefinition funDef in IAST.Funs.Values)
                        Console.WriteLine(funDef.Name);
                    continue;
                }

                if (text == ":sts")
                {
                    foreach (StructDefinition structDef in IAST.Structs.Values)
                        Console.WriteLine(structDef.Name);
                    continue;
                }

                if (string.IsNullOrEmpty(text))
                    break;

                var inputStream = new AntlrInputStream(text);
                var lexer = new llLexer(inputStream);
                var tokenStream = new CommonTokenStream(lexer);
                var parser = new llParser(tokenStream);
                var tmp = new StructDefinitionVisitor(file).VisitCompileUnit(parser.compileUnit());

                inputStream = new AntlrInputStream(text);
                lexer = new llLexer(inputStream);
                tokenStream = new CommonTokenStream(lexer);
                parser = new llParser(tokenStream);
                tmp = new FunctionDefinitionVisitor(file).VisitCompileUnit(parser.compileUnit());

                inputStream = new AntlrInputStream(text);
                lexer = new llLexer(inputStream);
                tokenStream = new CommonTokenStream(lexer);
                parser = new llParser(tokenStream);
                var ast = new BuildAstVisitor(file).VisitCompileUnit(parser.compileUnit());
                var value = ast.Eval();

                if (value != null)
                    Console.WriteLine("= {0}", value.ToString());

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

                var inputStream = new AntlrInputStream(text);
                var lexer = new llLexer(inputStream);
                var tokenStream = new CommonTokenStream(lexer);
                var parser = new llParser(tokenStream);
                var tmp = new StructDefinitionVisitor(file).VisitCompileUnit(parser.compileUnit());

                inputStream = new AntlrInputStream(text);
                lexer = new llLexer(inputStream);
                tokenStream = new CommonTokenStream(lexer);
                parser = new llParser(tokenStream);
                tmp = new FunctionDefinitionVisitor(file).VisitCompileUnit(parser.compileUnit());

                inputStream = new AntlrInputStream(text);
                lexer = new llLexer(inputStream);
                tokenStream = new CommonTokenStream(lexer);
                parser = new llParser(tokenStream);
                var ast = new BuildAstVisitor(file).VisitCompileUnit(parser.compileUnit());
                var assemblerGenerator = new AssemblerGenerator(file);
                assemblerGenerator.GenerateAssember(ast);
                assemblerGenerator.PrintAssember();
            }
        }

        static void CompilerMode(string inputFile)
        {
            Console.WriteLine($"Compiling {inputFile}...");

            var inputStream = new AntlrFileStream(inputFile);
            var lexer = new llLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new llParser(tokenStream);
            var tmp = new StructDefinitionVisitor(inputFile).VisitCompileUnit(parser.compileUnit());

            inputStream = new AntlrFileStream(inputFile);
            lexer = new llLexer(inputStream);
            tokenStream = new CommonTokenStream(lexer);
            parser = new llParser(tokenStream);
            tmp = new FunctionDefinitionVisitor(inputFile).Visit(parser.compileUnit());

            inputStream = new AntlrFileStream(inputFile);
            lexer = new llLexer(inputStream);
            tokenStream = new CommonTokenStream(lexer);
            parser = new llParser(tokenStream);
            var ast = new BuildAstVisitor(inputFile).Visit(parser.compileUnit());
            var assemblerGenerator = new AssemblerGenerator(inputFile);

            assemblerGenerator.WriteToFile(inputFile, ast);
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
