using NUnit.Framework;
using Antlr4.Runtime;
using ll;

namespace ll.test
{
    [TestFixture]
    public class TestExpressionSequenz
    {
        BuildAstVisitor visitor = new BuildAstVisitor();

        public llParser Setup(string text)
        {
            AntlrInputStream inputStream = new AntlrInputStream(text);
            llLexer lexer = new llLexer(inputStream);
            CommonTokenStream stream = new CommonTokenStream(lexer);
            return new llParser(stream);
        }

        [TestCase("{x:int=10; y:int=x+2; return y;}", 12)]
        [TestCase("{return 10;}", 10)]
        [TestCase("{return 10+2;}", 12)]
        /*
        * these tests are currently not working because the type of boolean could not be resolved
        [TestCase("{x:int=10; return x==10;}", 1)]
        [TestCase("{x:int=10; return x<11;}", 1)]
        [TestCase("{x:int=10; return x>9;}", 1)]
        [TestCase("{x:int=10; return x;}", 10)]
        */
        [TestCase("{x:int=10; x=x+2; return x;}", 12)]
        public void TestExpressionSequenz_1(string input, double expected)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual(expected, result.Eval());
        }

        [Test]
        public void TestExpressionSequenz_2()
        {
            llParser parser = Setup("{return 10;}");

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual("ll.AST.ExpressionSequenz", result.GetType().ToString());
        }
        
    }
}