using NUnit.Framework;
using Antlr4.Runtime;
using LL.AST;
using System;
using System.Globalization;

namespace LL.test
{
    [TestFixture]
    public class TestNumericExpression
    {
        BuildAstVisitor visitor = new BuildAstVisitor("UnitTests");

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

            Assert.AreEqual(Int32.Parse(input), (result.Eval() as IntLit).Value);
        }

        [Test]
        public void TestIntLit_2()
        {
            llParser parser = Setup("10");

            var result = visitor.Visit(parser.compileUnit()) as ProgramNode;

            Assert.AreEqual("LL.AST.IntLit", result.CompositUnit.GetType().ToString());
        }

        [TestCase("0.5")]
        [TestCase("-100.5")]
        [TestCase("0.0")]
        [TestCase("+10.0")]
        public void TestDoubleLit_1(string input)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual(Double.Parse(input, new CultureInfo("en-US").NumberFormat), (result.Eval() as DoubleLit).Value);
        }

        [Test]
        public void TestDoubleLit_2()
        {
            llParser parser = Setup("10.0");

            var result = visitor.Visit(parser.compileUnit()) as ProgramNode;

            Assert.AreEqual("LL.AST.DoubleLit", result.CompositUnit.GetType().ToString());
        }
    }
}