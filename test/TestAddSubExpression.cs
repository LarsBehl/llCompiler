using NUnit.Framework;
using Antlr4.Runtime;
using ll;

namespace test
{
    [TestFixture]
    public class TestAddSubExpression
    {
        BuildAstVisitor visitor = new BuildAstVisitor();

        public llParser Setup(string text)
        {
            AntlrInputStream inputStream = new AntlrInputStream(text);
            llLexer lexer = new llLexer(inputStream);
            CommonTokenStream stream = new CommonTokenStream(lexer);
            return new llParser(stream);
        }

        [TestCase("1+1", 2)]
        [TestCase("-1+1", 0)]
        [TestCase("1+1+1", 3)]
        [TestCase("-1+-1", -2)]
        public void TestAddExpresion_1(string input, int expected)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.expression());

            Assert.AreEqual(expected, result.Eval());
        }

        [TestCase("1.0+1.0", 2.0)]
        [TestCase("-1.0+1.0", 0.0)]
        [TestCase("1.0+1+1", 3.0)]
        [TestCase("-1.0+-1.0", -2.0)]
        public void TestAddExpresion_2(string input, double expected)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.expression());

            Assert.AreEqual(expected, result.Eval());
        }

        [Test]
        public void TestAddExpresion_3()
        {
            llParser parser = Setup("1+1");

            var result = visitor.Visit(parser.expression());

            Assert.AreEqual("ll.AST.AddExpr", result.GetType().ToString());
        }

        [TestCase("1-1", 0)]
        [TestCase("-2-2", -4)]
        [TestCase("-2--2", 0)]
        public void TestSubExpression_1(string input, int expected)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.expression());

            Assert.AreEqual(expected, result.Eval());
        }

        [TestCase("1.0-1.0", 0.0)]
        public void TestSubExpression_2(string input, double expected)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.expression());

            Assert.AreEqual(expected, result.Eval());
        }

        [Test]
        public void TestSubExpression_3()
        {
            llParser parser = Setup("1-1");

            var result = visitor.Visit(parser.expression());

            Assert.AreEqual("ll.AST.SubExpr", result.GetType().ToString());
        }
    }
}