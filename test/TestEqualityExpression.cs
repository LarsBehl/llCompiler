using NUnit.Framework;
using Antlr4.Runtime;
using ll.AST;
using System;

namespace ll.test
{
    [TestFixture]
    public class TestEqualityExpression
    {
        BuildAstVisitor visitor = new BuildAstVisitor();

        public llParser Setup(string text)
        {
            AntlrInputStream inputStream = new AntlrInputStream(text);
            llLexer lexer = new llLexer(inputStream);
            CommonTokenStream stream = new CommonTokenStream(lexer);
            return new llParser(stream);
        }

        [TestCase("2==2", true)]
        [TestCase("3==2", false)]
        [TestCase("-2==2", false)]
        [TestCase("-2==-2", true)]
        [TestCase("(2>1)==true", true)]
        [TestCase("null == null", true)]
        public void TestEqualityExpression_1(string input, bool expected)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual(expected, (result.Eval() as BoolLit).value);
        }

        [Test]
        public void TestEqualityExpression_2()
        {
            llParser parser = Setup("2==2");

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual("ll.AST.EqualityExpr", result.GetType().ToString());
        }

        [TestCase("(2>1)==5")]
        [TestCase("5>3==2")]
        public void TestEqualityExpression_3(string input)
        {
            llParser parser = Setup(input);

            Assert.Throws<ArgumentException>(() => visitor.Visit(parser.compileUnit()));
        }
    }
}