using System;
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

                bool isCircular = StructDefinitionVisitor.ProgData.ContainsCircularDependency(out List<ProgramNode> nodes);

                if(isCircular)
                    throw new CircularDependencyException(nodes, fileLocation);

                foreach(ProgramNode node in nodes)
                {
                    new FunctionDefinitionVisitor(node).VisitCompileUnit(node.Parser.compileUnit());
                    node.Parser.Reset();

                    new BuildAstVisitor(node).VisitCompileUnit(node.Parser.compileUnit());
                }
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