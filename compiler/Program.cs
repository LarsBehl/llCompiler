using System;
using System.IO;
using Antlr4.Runtime;


namespace ll
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                Console.Write("> ");
                string text = Console.ReadLine();

                if(string.IsNullOrEmpty(text))
                    break;
                
                var inputStream = new AntlrInputStream(new StringReader(text));
                var lexer = new llLexer(inputStream);
                var tokenStream = new CommonTokenStream(lexer);
                var parser = new llParser(tokenStream);

                try
                {
                    var cst = parser.compileUnit();
                    var ast = new BuildAstVisitor().VisitCompileUnit(cst);
                    var value = ast.Eval();

                    Console.WriteLine("= {0}", value);
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }

                Console.WriteLine();
            }
        }
    }
}
