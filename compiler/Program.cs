using System;
using System.IO;
using Antlr4.Runtime;
using ll.AST;

namespace ll
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.Write("> ");
                string text = Console.ReadLine();

                if (text == ":fs")
                {
                    foreach (FunctionDefinition funDef in IAST.funs.Values)
                        Console.WriteLine(funDef.name);
                    break;
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
    }
}
