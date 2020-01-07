using System;
using System.IO;
using Antlr4.Runtime;
using ll.AST;
using ll.assembler;

namespace ll
{
    class Program
    {
        static void InterpreterMode()
        {
            Console.WriteLine("Running in Interpreter Mode\n");

            var fileStream = new AntlrFileStream("../test/programs/TestFile1.ll");
            var fileLexer = new llLexer(fileStream);
            var fileTokenStream = new CommonTokenStream(fileLexer);
            var fileParser = new llParser(fileTokenStream);
            var tmp1 = new FunctionDefinitionVisitor().VisitProgram(fileParser.program());

            fileStream = new AntlrFileStream("../test/programs/TestFile1.ll");
            fileLexer = new llLexer(fileStream);
            fileTokenStream = new CommonTokenStream(fileLexer);
            fileParser = new llParser(fileTokenStream);
            var tmpAst = new BuildAstVisitor().VisitProgram(fileParser.program());

            while (true)
            {
                Console.Write("> ");
                string text = Console.ReadLine();

                if (text == ":fs")
                {
                    foreach (FunctionDefinition funDef in IAST.funs.Values)
                        Console.WriteLine(funDef.name);
                    continue;
                }

                if (string.IsNullOrEmpty(text))
                    break;

                try
                {
                    var inputStream = new AntlrInputStream(text);
                    var lexer = new llLexer(inputStream);
                    var tokenStream = new CommonTokenStream(lexer);
                    var parser = new llParser(tokenStream);
                    var tmp = new FunctionDefinitionVisitor().VisitCompileUnit(parser.compileUnit());

                    inputStream = new AntlrInputStream(text);
                    lexer = new llLexer(inputStream);
                    tokenStream = new CommonTokenStream(lexer);
                    parser = new llParser(tokenStream);
                    var ast = new BuildAstVisitor().VisitCompileUnit(parser.compileUnit());
                    var value = ast.Eval();

                    if (value != null)
                        Console.WriteLine("= {0}", value.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                Console.WriteLine();
            }
        }

        static void InteractiveCompilerMode()
        {
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
                var tmp = new FunctionDefinitionVisitor().VisitCompileUnit(parser.compileUnit());

                inputStream = new AntlrInputStream(text);
                lexer = new llLexer(inputStream);
                tokenStream = new CommonTokenStream(lexer);
                parser = new llParser(tokenStream);
                var ast = new BuildAstVisitor().VisitCompileUnit(parser.compileUnit());
                var assemblerGenerator = new GenAssembler();
                assemblerGenerator.GenerateAssember(ast);
            }
        }

        static void CompilerMode(string inputFile)
        {
            Console.WriteLine($"Compiling {inputFile}...");

            var inputStream = new AntlrFileStream(inputFile);
            var lexer = new llLexer(inputStream);
            var tokenStream = new CommonTokenStream(lexer);
            var parser = new llParser(tokenStream);
            var tmp = new FunctionDefinitionVisitor().Visit(parser.compileUnit());

            inputStream = new AntlrFileStream(inputFile);
            lexer = new llLexer(inputStream);
            tokenStream = new CommonTokenStream(lexer);
            parser = new llParser(tokenStream);
            var ast = new BuildAstVisitor().Visit(parser.compileUnit());
            var assemblerGenerator = new GenAssembler();

            assemblerGenerator.WriteToFile(inputFile, ast);
        }

        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                if (args[0] == "-i")
                    Program.InterpreterMode();
                if(args[0] == "-c" && args.Length > 1)
                    Program.CompilerMode(args[1]);
                else
                    Console.WriteLine($"unknown flag {args[0]}");
            }
            else
            {
                Program.InteractiveCompilerMode();
            }
        }
    }
}
