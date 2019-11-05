using NUnit.Framework;
using Antlr4.Runtime;
using ll;

namespace ll.test
{
    [TestFixture]
    public class TestLessExpression
    {
        BuildAstVisitor visitor = new BuildAstVisitor();

        public llParser Setup(string text)
        {
            AntlrInputStream inputStream = new AntlrInputStream(text);
            llLexer lexer = new llLexer(inputStream);
            CommonTokenStream stream = new CommonTokenStream(lexer);
            return new llParser(stream);
        }

        [TestCase("2 < 3", 1)]
        [TestCase("-2 < 3", 1)]
        [TestCase("3 < 2", 0)]
        [TestCase("2+3<4", 0)]
        [TestCase("2 < 4*2", 1)]
        [TestCase("2/3 < 1/2", 0)]
        public void TestLessExpression_1(string input, double expected)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual(expected, result.Eval());
        }

        public void TestLessExpression_2()
        {
            llParser parser = Setup("2 < 3");

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual("ll.AST.LessExpr", result.GetType().ToString());
        }
    }
}