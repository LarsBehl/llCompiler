using NUnit.Framework;

using Antlr4.Runtime;

using LL.AST;
using LL.Types;

namespace LL.Test
{
    [TestFixture]
    public class TestStringLiteral
    {
        BuildAstVisitor visitor = new BuildAstVisitor("UnitTests");

        private llParser Setup(string content)
        {
            return new llParser(new CommonTokenStream(new llLexer(new AntlrInputStream(content))));
        }

        [Test]
        public void TestStringLiteral1()
        {
            string input = "\"Hallo Welt\"";
            llParser parser = Setup(input);
            ProgramNode result = visitor.Visit(parser.compileUnit()) as ProgramNode;

            Assert.AreEqual(typeof(StringLit), result.CompositUnit.GetType());
            Assert.NotNull((result.CompositUnit as StringLit).Value);
            Assert.True(result.CompositUnit.Type is CharArrayType);
        }
    }
}