using NUnit.Framework;
using Antlr4.Runtime;
using ll;

namespace ll.test
{
    [TestFixture]
    public class TestGreaterExpression
    {
        BuildAstVisitor visitor = new BuildAstVisitor();

        public llParser Setup(string text)
        {
            AntlrInputStream inputStream = new AntlrInputStream(text);
            llLexer lexer = new llLexer(inputStream);
            CommonTokenStream stream = new CommonTokenStream(lexer);
            return new llParser(stream);
        }

        [TestCase("2 > 3", 0)]
        [TestCase("-2 > 3", 0)]
        [TestCase("3 > 2", 1)]
        [TestCase("2+3 > 4", 1)]
        [TestCase("2 > 4*2", 0)]
        [TestCase("2/3 > 1/2", 1)]
        public void TestGreaterExpression_1(string input, double expected)
        {
            llParser parser = Setup(input);

            var reslt = visitor.Visit(parser.compileUnit());

            Assert.AreEqual(expected, reslt.Eval());
        }

        [Test]
        public void TestGreaterExpression_2()
        {
            llParser parser = Setup("3 > 0");

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual("ll.AST.GreaterExpr", result.GetType().ToString());
        }
    }
}