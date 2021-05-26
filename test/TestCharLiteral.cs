using NUnit.Framework;

using Antlr4.Runtime;

using LL.AST;

namespace LL.Test
{
    [TestFixture]
    public class TestCharLiteral
    {
        BuildAstVisitor visitor = new BuildAstVisitor("UnitTests");

        private llParser Setup(string content)
        {
            return new llParser(new CommonTokenStream(new llLexer(new AntlrInputStream(content))));
        }

        [TestCase("'c'", 'c')]
        [TestCase("'ü'", 'ü')]
        [TestCase("'-'", '-')]
        [TestCase("'ß'", 'ß')]
        [TestCase("'é'", 'é')]
        [TestCase("'\n'", '\n')]
        public void TestCharLiteral1(string input, char expected)
        {
            llParser parser = Setup(input);
            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual(expected, (result.Eval() as CharLit).Value);
        }
    }
}