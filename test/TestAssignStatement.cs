using NUnit.Framework;
using Antlr4.Runtime;
using ll;

namespace ll.test
{
    [TestFixture]
    public class TestAssignStatement
    {
        BuildAstVisitor visitor = new BuildAstVisitor();

        public llParser Setup(string text)
        {
            AntlrInputStream inputStream = new AntlrInputStream(text);
            llLexer lexer = new llLexer(inputStream);
            CommonTokenStream stream = new CommonTokenStream(lexer);
            return new llParser(stream);
        }

        [TestCase("x=2+2;", 0)]
        [TestCase("x=-2+2*2;", 0)]
        [TestCase("x=(-3+2)*2;", 0)]
        [TestCase("x=10;", 0)]
        [TestCase("x=1/2;", 0)]
        public void TestAssignStatement_1(string input, double expected)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual(expected, result.Eval());
        }

        [Test]
        public void TestAssignStatement_2()
        {
            llParser parser = Setup("x=2*2;");

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual("ll.AST.AssignStatement", result.GetType().ToString());
        }
    }
}