using NUnit.Framework;
using Antlr4.Runtime;
using ll;

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

        [TestCase("2==2", 1)]
        [TestCase("3==2", 0)]
        [TestCase("-2==2", 0)]
        [TestCase("-2==-2", 1)]
        public void TestEqualityExpression_1(string input, int expected)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.expression());

            Assert.AreEqual(expected, result.Eval());
        }

        [Test]
        public void TestEqualityExpression_2()
        {
            llParser parser = Setup("2==2");

            var result = visitor.Visit(parser.expression());

            Assert.AreEqual("ll.AST.EqualityExpr", result.GetType().ToString());
        }
    }
}