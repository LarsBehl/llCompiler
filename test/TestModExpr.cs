using NUnit.Framework;
using Antlr4.Runtime;
using LL;
using LL.AST;
using System;

namespace test
{
    [TestFixture]
    public class TestModExpr
    {
        BuildAstVisitor visitor = new BuildAstVisitor("UnitTests");

        public llParser Setup(string text)
        {
            AntlrInputStream inputStream = new AntlrInputStream(text);
            llLexer lexer = new llLexer(inputStream);
            CommonTokenStream stream = new CommonTokenStream(lexer);
            return new llParser(stream);
        }

        [TestCase("5%2", 1)]
        [TestCase("-1%3", -1)]
        [TestCase("4%2", 0)]
        public void TestModExpr_1(string input, int expected)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual(expected, (result.Eval() as IntLit).Value);
        }
    }
}