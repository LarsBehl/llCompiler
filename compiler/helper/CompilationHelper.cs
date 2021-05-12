using System;

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

                parser.Reset();
                result = new FunctionDefinitionVisitor(fileLocation, result).VisitCompileUnit(parser.compileUnit()) as ProgramNode;

                parser.Reset();
                result = new BuildAstVisitor(result).VisitCompileUnit(parser.compileUnit()) as ProgramNode;
            }
            catch (BaseCompilerException e)
            {
                PrintError(e);
                Environment.Exit(-1);
            }

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