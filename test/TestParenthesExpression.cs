using NUnit.Framework;
using Antlr4.Runtime;
using LL.AST;

namespace LL.Test
{
    [TestFixture]
    public class TestParenthesExpression
    {
        BuildAstVisitor visitor = new BuildAstVisitor("UnitTests");

        public llParser Setup(string text)
        {
            AntlrInputStream inputStream = new AntlrInputStream(text);
            llLexer lexer = new llLexer(inputStream);
            CommonTokenStream stream = new CommonTokenStream(lexer);
            return new llParser(stream);
        }

        [TestCase("(2)+2", 4)]
        [TestCase("(2+2)", 4)]
        [TestCase("(2+3)*2", 10)]
        public void TestParenthesExpression_1(string input, int expected)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual(expected, (result.Eval() as IntLit).Value);
        }

        [TestCase("(0+1.0)/2", 0.5)]
        [TestCase("(-2.5)", -2.5)]
        public void TestParenthesExpression_2(string input, double expected)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual(expected, (result.Eval() as DoubleLit).Value);
        }
    }
}