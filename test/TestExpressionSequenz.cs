using NUnit.Framework;
using Antlr4.Runtime;
using ll.AST;

namespace ll.test
{
    [TestFixture]
    public class TestExpressionSequenz
    {
        BuildAstVisitor visitor = new BuildAstVisitor();

        [SetUp]
        public void ClearEnv()
        {
            IAST.env = new System.Collections.Generic.Dictionary<string, IAST>();
        }

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
        [TestCase("{x:int=10; return x;}", 10)]
        [TestCase("{x:int=10; x=x+2; return x;}", 12)]
        [TestCase("{_x2_:int=10; return ++_x2_;}", 11)]
        [TestCase("{x:int=10; x+=32; return x;}", 42)]
        [TestCase("{x:int=45; x-=3; return x;}", 42)]
        [TestCase("{x:int=21; x*=2; return x;}", 42)]
        [TestCase("{x:int=84; x/=2; return x;}", 42)]
        public void TestExpressionSequenz_1(string input, int expected)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual(expected, (result.Eval() as IntLit).n);
        }

        [TestCase("{x:double=2.0; return x;}", 2.0)]
        [TestCase("{x:double; x=10.0; return x;}", 10.0)]
        [TestCase("{x:double=10.0; x+=32.0; return x;}", 42.0)]
        [TestCase("{x:double=45.0; x-=3.0; return x;}", 42.0)]
        [TestCase("{x:double=21.0; x*=2.0; return x;}", 42.0)]
        [TestCase("{x:double=84.0; x/=2.0; return x;}", 42.0)]
        public void TestExpressionSequenz_2(string input, double expected)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual(expected, (result.Eval() as DoubleLit).n);
        }

        [TestCase("{x:int=10; return x==10;}", true)]
        [TestCase("{x:int=10; return x<11;}", true)]
        [TestCase("{x:int=10; return x>9;}", true)]
        [TestCase("{x:bool=true; return x;}", true)]
        [TestCase("{x:bool=5<3; return x;}", false)]
        public void TestExpressionSequenz_3(string input, bool expected)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual(expected, (result.Eval() as BoolLit).value);
        }

        [Test]
        public void TestExpressionSequenz_4()
        {
            llParser parser = Setup("{return 10;}");

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual("ll.AST.BlockStatement", result.GetType().ToString());
        }
        
    }
}