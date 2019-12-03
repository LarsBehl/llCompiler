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
        bool once = false;

        public llParser Setup(string text)
        {
            AntlrInputStream inputStream = new AntlrInputStream(text);
            llLexer lexer = new llLexer(inputStream);
            CommonTokenStream stream = new CommonTokenStream(lexer);

            return new llParser(stream);
        }

        /*
        [TestCase("aCallsB(3)", 5)]
        [TestCase("id(5)", 5)]
        [TestCase("square(5)", 25)]
        [TestCase("fourtyTwo()", 42)]
        [TestCase("plusSeventeen(square(5))", 42)]
        [TestCase("fac(5)", 120)]
        public void TestFunctions_1(string funCall, int expected)
        {
            llParser parser;
            if (!once)
            {
                this.once = true;
                StreamReader reader = new StreamReader("C:/Users/larsb/source/hsrm/compilerBau/llCompiler/test/programs/TestFile1.ll");
                try
                {
                    string input = reader.ReadToEnd();

                    parser = Setup(input);
                    funDefVisitor.Visit(parser.program());

                    parser = Setup(input);
                    visitor.Visit(parser.program());
                }
                catch (IOException e)
                {
                    Console.WriteLine(e);
                }
            }

            parser = Setup(funCall);
            var tmp = visitor.Visit(parser.program()).Eval();
            Assert.AreEqual(expected, (tmp as IntLit).n);
        }

        [Test]
        public void TestFunctions_3()
        {
            llParser parser;

            StreamReader reader = new StreamReader("C:/Users/larsb/source/hsrm/compilerBau/llCompiler/test/programs/TestFile2.ll");
            try
            {
                string input = reader.ReadToEnd();

                parser = Setup(input);
                funDefVisitor.Visit(parser.program());

                parser = Setup(input);
                Assert.Throws<ArgumentException>(() => visitor.Visit(parser.program()));
            }
            catch(IOException e)
            {
                Console.WriteLine(e);
            }
        }
        */
    }
}