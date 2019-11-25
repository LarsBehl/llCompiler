using NUnit.Framework;
using Antlr4.Runtime;
using ll.AST;
using System.IO;
using System;

namespace ll.test
{
    [TestFixture]
    public class TestFunctions
    {
        BuildAstVisitor visitor = new BuildAstVisitor();
        FunctionDefinitionVisitor funDefVisitor = new FunctionDefinitionVisitor();

        public llParser Setup(string text)
        {
            AntlrInputStream inputStream = new AntlrInputStream(text);
            llLexer lexer = new llLexer(inputStream);
            CommonTokenStream stream = new CommonTokenStream(lexer);

            return new llParser(stream);
        }

        [Test]
        public void TestFunctions_1()
        {
            StreamReader reader = new StreamReader("C:/Users/larsb/source/hsrm/compilerBau/llCompiler/test/TestFile1.ll");
            try
            {
                string input = reader.ReadToEnd();
                
                llParser parser = Setup(input);
                funDefVisitor.Visit(parser.compileUnit());

                parser = Setup(input);

                //Assert.AreEqual(5, (visitor.Visit(parser.compileUnit()) as IntLit).n);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}