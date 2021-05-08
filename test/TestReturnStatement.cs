using NUnit.Framework;
using Antlr4.Runtime;
using LL.AST;

namespace LL.test
{
    [TestFixture]
    public class TestReturnStatement
    {
        BuildAstVisitor visitor = new BuildAstVisitor("UnitTests");

        public llParser Setup(string text)
        {
            AntlrInputStream inputStream = new AntlrInputStream(text);
            llLexer lexer = new llLexer(inputStream);
            CommonTokenStream stream = new CommonTokenStream(lexer);
            return new llParser(stream);
        }

        
        [TestCase("return 2;", 2)]
        [TestCase("return 2+2;", 4)]
        [TestCase("return 2*2;", 4)]
        [TestCase("return (2*2);", 4)]
        public void TestReturnExpression_1(string input, int expected)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.statement());

            Assert.AreEqual(expected, (result.Eval() as IntLit).Value);
        }

        [TestCase("return 1/2.0;", 0.5)]
        public void TestReturnExpression_2(string input, double expected)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.statement());

            Assert.AreEqual(expected, (result.Eval() as DoubleLit).Value);
        }
        
        [Test]
        public void TestReturnExpression_3()
        {
            llParser parser = Setup("return 2;");

            var result = visitor.Visit(parser.program()) as ProgramNode;

            Assert.AreEqual("LL.AST.ReturnStatement", result.CompositUnit.GetType().ToString());
        }
        
    }
}