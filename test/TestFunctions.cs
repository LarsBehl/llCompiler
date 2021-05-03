using NUnit.Framework;
using Antlr4.Runtime;
using LL.AST;
using System.IO;
using System;
using LL.Exceptions;

namespace LL.test
{
    [TestFixture]
    public class TestFunctions
    {
        BuildAstVisitor visitor = new BuildAstVisitor("UnitTests");
        FunctionDefinitionVisitor funDefVisitor = new FunctionDefinitionVisitor("Unit Tests");
        bool once = false;
        bool t3 = false;

        public llParser Setup(string text)
        {
            AntlrInputStream inputStream = new AntlrInputStream(text);
            llLexer lexer = new llLexer(inputStream);
            CommonTokenStream stream = new CommonTokenStream(lexer);

            return new llParser(stream);
        }


        [TestCase("aCallsB(3)", 5)]
        [TestCase("id(5)", 5)]
        [TestCase("square(5)", 25)]
        [TestCase("fourtyTwo()", 42)]
        [TestCase("plusSeventeen(square(5))", 42)]
        [TestCase("fac(5)", 120)]
        [TestCase("facIter(5)", 120)]
        [TestCase("equalsArray()", 42)]
        [TestCase("equalsArrayNull()", 42)]
        [TestCase("equalsNullArrayNull()", 42)]
        [TestCase("notEqualsArrayNull()", 42)]
        [TestCase("notEqualsArray()", 42)]
        [TestCase("notEqualsNullArrayNull()", 42)]
        public void TestFunctions_1(string funCall, int expected)
        {
            llParser parser;
            if (!once)
            {
                this.once = true;
                StreamReader reader = new StreamReader("../../../programs/TestFile1.ll");
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
            Assert.AreEqual(expected, (tmp as IntLit).Value);
        }

        [Test]
        public void TestFunctions_2()
        {
            llParser parser;
            StreamReader reader = new StreamReader("../../../programs/TestFile3.ll");
            try
            {
                string input = reader.ReadToEnd();

                parser = Setup(input);
                funDefVisitor.Visit(parser.program());

                parser = Setup(input);
                Assert.Throws<MissingReturnStatementException>(() => visitor.Visit(parser.program()));
            }
            catch (IOException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}