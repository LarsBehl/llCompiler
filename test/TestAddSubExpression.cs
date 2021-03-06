using NUnit.Framework;
using Antlr4.Runtime;
using LL;
using LL.AST;
using System;

namespace test
{
    [TestFixture]
    public class TestAddSubExpression
    {
        BuildAstVisitor visitor = new BuildAstVisitor("UnitTests");

        public llParser Setup(string text)
        {
            AntlrInputStream inputStream = new AntlrInputStream(text);
            llLexer lexer = new llLexer(inputStream);
            CommonTokenStream stream = new CommonTokenStream(lexer);
            return new llParser(stream);
        }

        [TestCase("1+1", 2)]
        [TestCase("-1+1", 0)]
        [TestCase("1+1+1", 3)]
        [TestCase("-1+-1", -2)]
        public void TestAddExpresion_1(string input, int expected)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual(expected, (result.Eval() as IntLit).Value);
        }

        [TestCase("1.0+1.0", 2.0)]
        [TestCase("-1.0+1.0", 0.0)]
        [TestCase("1.0+1+1", 3.0)]
        [TestCase("-1.0+-1.0", -2.0)]
        public void TestAddExpresion_2(string input, double expected)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual(expected, (result.Eval() as DoubleLit).Value);
        }

        [Test]
        public void TestAddExpresion_3()
        {
            llParser parser = Setup("1+1");

            var result = visitor.Visit(parser.compileUnit()) as ProgramNode;

            Assert.AreEqual("LL.AST.AddExpr", result.CompositUnit.GetType().ToString());
        }

        [TestCase("(5==3)+2")]
        [TestCase("(5>3)+2")]
        [TestCase("(5<3)+2")]
        public void TestAddExpression_4(string input)
        {
            llParser parser = Setup(input);

            Assert.Throws<ArgumentException>(() => visitor.Visit(parser.compileUnit()));
        }

        [TestCase("1-1", 0)]
        [TestCase("-2-2", -4)]
        [TestCase("-2--2", 0)]
        public void TestSubExpression_1(string input, int expected)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual(expected, (result.Eval() as IntLit).Value);
        }

        [TestCase("1.0-1.0", 0.0)]
        public void TestSubExpression_2(string input, double expected)
        {
            llParser parser = Setup(input);

            var result = visitor.Visit(parser.compileUnit());

            Assert.AreEqual(expected, (result.Eval() as DoubleLit).Value);
        }

        [Test]
        public void TestSubExpression_3()
        {
            llParser parser = Setup("1-1");

            var result = visitor.Visit(parser.compileUnit()) as ProgramNode;

            Assert.AreEqual("LL.AST.SubExpr", result.CompositUnit.GetType().ToString());
        }

        [TestCase("(5==3)-2")]
        [TestCase("(5>3)-2")]
        [TestCase("(5<3)-2")]
        public void TestSubExpression_4(string input)
        {
            llParser parser = Setup(input);

            Assert.Throws<ArgumentException>(() => visitor.Visit(parser.compileUnit()));
        }
    }
}