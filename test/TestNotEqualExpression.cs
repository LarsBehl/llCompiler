using NUnit.Framework;
using Antlr4.Runtime;
using LL.AST;
using System;

namespace LL.test
{
    [TestFixture]
    public class TestNotEqualExpression
    {
        BuildAstVisitor visitor = new BuildAstVisitor("UnitTests");

        public llParser Setup(string text)
        {
            AntlrInputStream inputStream = new AntlrInputStream(text);
            llLexer lexer = new llLexer(inputStream);
            CommonTokenStream stream = new CommonTokenStream(lexer);
            return new llParser(stream);
        }

        [TestCase("1 != 2", true)]
        [TestCase("1 != 1", false)]
        [TestCase("1.0 != 2", true)]
        [TestCase("2.0 != 1", true)]
        [TestCase("true != false", true)]
        [TestCase("3 < 5 != !true", true)]
        [TestCase("null != null", false)]
        public void TestNotEqualExpression_1(string input, bool expected)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual(expected, (result.Eval() as BoolLit).Value);
        }

        [Test]
        public void TestNotEqualExpression_2()
        {
            llParser parser = Setup("true != 5");

            Assert.Throws<ArgumentException>(() => visitor.Visit(parser.compileUnit()));
        }
    }
}