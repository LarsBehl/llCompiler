using NUnit.Framework;
using Antlr4.Runtime;
using ll;

namespace test
{
    [TestFixture]
    public class TestAssignExpression
    {
        BuildAstVisitor visitor = new BuildAstVisitor();

        public llParser Setup(string text)
        {
            AntlrInputStream inputStream = new AntlrInputStream(text);
            llLexer lexer = new llLexer(inputStream);
            CommonTokenStream stream = new CommonTokenStream(lexer);
            return new llParser(stream);
        }

        [TestCase("x=2+2", 4)]
        [TestCase("x=-2+2*2", 2)]
        [TestCase("x=(-3+2)*2", -2)]
        [TestCase("x=10", 10)]
        [TestCase("x=1/2", 0.5)]
        public void TestAssignExpression_1(string input, double expected)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.expression());

            Assert.AreEqual(expected, result.Eval());
        }

        [Test]
        public void TestAssignExpression_2()
        {
            llParser parser = Setup("x=2*2");

            var result = visitor.Visit(parser.expression());

            Assert.AreEqual("ll.AssignExpr", result.GetType().ToString());
        }
    }
}