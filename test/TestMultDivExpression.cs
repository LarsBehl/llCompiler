using NUnit.Framework;
using Antlr4.Runtime;
using ll;

namespace ll.test
{
    [TestFixture]
    public class TestMultDivExpression
    {
        BuildAstVisitor visitor = new BuildAstVisitor();

        public llParser Setup(string text)
        {
            AntlrInputStream inputStream = new AntlrInputStream(text);
            llLexer lexer = new llLexer(inputStream);
            CommonTokenStream stream = new CommonTokenStream(lexer);
            return new llParser(stream);
        }

        [TestCase("1*1", 1)]
        [TestCase("3*5", 15)]
        [TestCase("-2*5", -10)]
        [TestCase("-2*-5", 10)]
        [TestCase("2*2*2", 8)]
        [TestCase("2*0", 0)]
        [TestCase("2*3+2", 8)]
        [TestCase("2+3*2", 8)]
        public void TestMultExpression_1(string input, int expected)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual(expected, result.Eval());
        }

        [TestCase("2.5*2", 5.0)]
        [TestCase("0.5*0.5", 0.25)]
        [TestCase("-0.5*2", -1.0)]
        public void TestMultExpression_2(string input, double expected)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual(expected, result.Eval());
        }

        [Test]
        public void TestMultExpression_3()
        {
            llParser parser = Setup("2*2");

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual("ll.AST.MultExpr", result.GetType().ToString());
        }

        [TestCase("6/3", 2)]
        [TestCase("1/2", 0.5)]
        [TestCase("1.5/2", 0.75)]
        [TestCase("-2/2", -1)]
        [TestCase("2/2", 1)]
        [TestCase("-1/-10", 0.1)]
        public void TestDivExpression_1(string input, double expected)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual(expected, result.Eval());
        }

        [Test]
        public void TestDivExpression_2()
        {
            llParser parser = Setup("2/2");

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual("ll.AST.DivExpr", result.GetType().ToString());
        }
    }
}