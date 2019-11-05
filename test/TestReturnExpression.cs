using NUnit.Framework;
using Antlr4.Runtime;
using ll;

namespace ll.test
{
    [TestFixture]
    public class TestReturnExpression
    {
        BuildAstVisitor visitor = new BuildAstVisitor();

        public llParser Setup(string text)
        {
            AntlrInputStream inputStream = new AntlrInputStream(text);
            llLexer lexer = new llLexer(inputStream);
            CommonTokenStream stream = new CommonTokenStream(lexer);
            return new llParser(stream);
        }

        
        [TestCase("return 2;", 2)]
        [TestCase("return 2+2;", 4)]
        [TestCase("return 1/2;", 0.5)]
        [TestCase("return 2*2;", 4)]
        [TestCase("return (2*2);", 4)]
        public void TestReturnExpression_1(string input, double expected)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.returnExpression());

            Assert.AreEqual(expected, result.Eval());
        }

        
        
        [Test]
        public void TestReturnExpression_2()
        {
            llParser parser = Setup("return 2;");

            var result = visitor.Visit(parser.returnExpression());

            Assert.AreEqual("ll.AST.ReturnExpr", result.GetType().ToString());
        }
        
    }
}