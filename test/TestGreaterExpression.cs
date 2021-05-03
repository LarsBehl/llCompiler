using NUnit.Framework;
using Antlr4.Runtime;
using LL.AST;

namespace LL.test
{
    [TestFixture]
    public class TestGreaterExpression
    {
        BuildAstVisitor visitor = new BuildAstVisitor("UnitTests");

        public llParser Setup(string text)
        {
            AntlrInputStream inputStream = new AntlrInputStream(text);
            llLexer lexer = new llLexer(inputStream);
            CommonTokenStream stream = new CommonTokenStream(lexer);
            return new llParser(stream);
        }

        [TestCase("2 > 3", false)]
        [TestCase("-2 > 3", false)]
        [TestCase("3 > 2", true)]
        [TestCase("2+3 > 4", true)]
        [TestCase("2 > 4*2", false)]
        [TestCase("2/3 > 1/2", false)]
        [TestCase("1/2 >= 1/2", true)]
        public void TestGreaterExpression_1(string input, bool expected)
        {
            llParser parser = Setup(input);

            var reslt = visitor.Visit(parser.compileUnit());

            Assert.AreEqual(expected, (reslt.Eval() as BoolLit).Value);
        }

        [Test]
        public void TestGreaterExpression_2()
        {
            llParser parser = Setup("3 > 0");

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual("LL.AST.GreaterExpr", result.GetType().ToString());
        }
    }
}