using NUnit.Framework;
using Antlr4.Runtime;
using ll;

namespace test
{
    [TestFixture]
    public class TestExpressionSequenz
    {
        BuildAstVisitor visitor = new BuildAstVisitor();

        public llParser Setup(string text)
        {
            AntlrInputStream inputStream = new AntlrInputStream(text);
            llLexer lexer = new llLexer(inputStream);
            CommonTokenStream stream = new CommonTokenStream(lexer);
            return new llParser(stream);
        }

        [TestCase("{x=10; y=x+2; return y;}", 12)]
        [TestCase("{return 10;}", 10)]
        [TestCase("{return 10+2;}", 12)]
        public void TestExpressionSequenz_1(string input, double expected)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.expressionSequenz());

            Assert.AreEqual(expected, result.Eval());
        }

        [Test]
        public void TestExpressionSequenz_2()
        {
            llParser parser = Setup("{x=10 return x;}");

            var result = visitor.Visit(parser.expressionSequenz());

            Assert.AreEqual("ll.ExpressionSequenz", result.GetType().ToString());
        }
    }
}