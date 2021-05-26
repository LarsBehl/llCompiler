using NUnit.Framework;

using Antlr4.Runtime;

// TODO add more unit tests
namespace LL.Test
{
    [TestFixture]
    public class TestArrays
    {
        BuildAstVisitor visitor = new BuildAstVisitor("UnitTests");

        private llParser Setup(string content)
        {
            return new llParser(new CommonTokenStream(new llLexer(new AntlrInputStream(content))));
        }

        [TestCase("x: char[];")]
        [TestCase("y: char[] = new char[5];")]
        [TestCase("z: int[];")]
        [TestCase("a: int[] = new int[2];")]
        [TestCase("b: double[];")]
        [TestCase("c: double[] = new double[42];")]
        [TestCase("d: bool[];")]
        [TestCase("e: bool[] = new bool[17];")]
        public void TestArrays1(string input)
        {
            llParser parser = Setup(input);
            var result = visitor.Visit(parser.compileUnit());

            Assert.NotNull(result);
        }
    }
}