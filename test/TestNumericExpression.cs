using NUnit.Framework;
using Antlr4.Runtime;
using ll.AST;
using System;
using System.Globalization;

namespace ll.test
{
    [TestFixture]
    public class TestNumericExpression
    {
        BuildAstVisitor visitor = new BuildAstVisitor();

        public llParser Setup(string text)
        {
            AntlrInputStream inputStream = new AntlrInputStream(text);
            llLexer lexer = new llLexer(inputStream);
            CommonTokenStream stream = new CommonTokenStream(lexer);
            return new llParser(stream);
        }

        [TestCase("5")]
        [TestCase("-100")]
        [TestCase("0")]
        [TestCase("+10")]
        public void TestIntLit_1(string input)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual(Int32.Parse(input), (result.Eval() as IntLit).n);
        }

        [Test]
        public void TestIntLit_2()
        {
            llParser parser = Setup("10");

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual("ll.AST.IntLit", result.GetType().ToString());
        }

        [TestCase("0.5")]
        [TestCase("-100.5")]
        [TestCase("0.0")]
        [TestCase("+10.0")]
        public void TestDoubleLit_1(string input)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual(Double.Parse(input, new CultureInfo("en-US").NumberFormat), (result.Eval() as DoubleLit).n);
        }

        [Test]
        public void TestDoubleLit_2()
        {
            llParser parser = Setup("10.0");

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual("ll.AST.DoubleLit", result.GetType().ToString());
        }
    }
}