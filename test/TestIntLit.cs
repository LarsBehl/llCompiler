using NUnit.Framework;
using Antlr4.Runtime;
using ll;

namespace test
{
    [TestFixture]
    public class TestIntLit
    {

        public llParser Setup(string text)
        {
            AntlrInputStream inputStream = new AntlrInputStream(text);
            llLexer lexer = new llLexer(inputStream);
            CommonTokenStream stream = new CommonTokenStream(lexer);
            return new llParser(stream);
        }

        [Test]
        public void TestIntLit_1()
        {
            llParser parser = Setup("5");
            BuildAstVisitor visitor = new BuildAstVisitor();

            var result = visitor.Visit(parser.numericExpression());

            Assert.AreEqual(5, result.Eval());
        }
    }
}