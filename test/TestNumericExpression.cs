using NUnit.Framework;
using Antlr4.Runtime;
using ll;
using System;
using System.Globalization;

namespace test
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

            var result = visitor.Visit(parser.numericExpression());

            Assert.AreEqual(Int32.Parse(input), result.Eval());
        }

        [Test]
        public void TestIntLit_2()
        {
            llParser parser = Setup("10");

            var result = visitor.Visit(parser.numericExpression());

            Assert.AreEqual("ll.IntLit", result.GetType().ToString());
        }

        [TestCase("0.5")]
        [TestCase("-100.5")]
        [TestCase("0.0")]
        [TestCase("+10.0")]
        public void TestDoubleLit_1(string input)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.numericExpression());

            Assert.AreEqual(Double.Parse(input, new CultureInfo("en-US").NumberFormat), result.Eval());
        }

        [Test]
        public void TestDoubleLit_2()
        {
            llParser parser = Setup("10.0");

            var result = visitor.Visit(parser.numericExpression());

            Assert.AreEqual("ll.DoubleLit", result.GetType().ToString());
        }

        [TestCase("x")]
        [TestCase("+")]
        [TestCase("-")]
        public void TestNumericExpression_1(string input)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.numericExpression());

            Assert.IsNull(result);
        }
    }
}