using NUnit.Framework;
using Antlr4.Runtime;
using LL.AST;

namespace LL.test
{
    [TestFixture]
    public class TestLessExpression
    {
        BuildAstVisitor visitor = new BuildAstVisitor("UnitTests");

        public llParser Setup(string text)
        {
            AntlrInputStream inputStream = new AntlrInputStream(text);
            llLexer lexer = new llLexer(inputStream);
            CommonTokenStream stream = new CommonTokenStream(lexer);
            return new llParser(stream);
        }

        [TestCase("2 < 3", true)]
        [TestCase("-2 < 3", true)]
        [TestCase("3 < 2", false)]
        [TestCase("2+3<4", false)]
        [TestCase("2 < 4*2", true)]
        [TestCase("2/3 < 1/2", false)]
        [TestCase("1/2 <= 1/2", true)]
        public void TestLessExpression_1(string input, bool expected)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual(expected, (result.Eval() as BoolLit).Value);
        }

        public void TestLessExpression_2()
        {
            llParser parser = Setup("2 < 3");

            var result = visitor.Visit(parser.compileUnit()) as ProgramNode;

            Assert.AreEqual("LL.AST.LessExpr", result.CompositUnit.GetType().ToString());
        }
    }
}