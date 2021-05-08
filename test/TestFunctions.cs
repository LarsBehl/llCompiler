using NUnit.Framework;
using Antlr4.Runtime;
using LL.AST;
using System.IO;
using System;
using LL.Exceptions;

namespace LL.test
{
    [TestFixture]
    public class TestFunctions
    {
        private BuildAstVisitor BuildAstVisitor;
        private static readonly string PROGRAM_PATH = "../../../programs/TestFile1.ll";
        bool once = false;
        ProgramNode Prog;

        public void Execute(string text)
        {
            llParser parser = new llParser(new CommonTokenStream(new llLexer(new AntlrInputStream(text))));
            parser.RemoveParseListeners();
            parser.AddErrorListener(new ErrorListener(PROGRAM_PATH));
            
            this.Prog = this.BuildAstVisitor.VisitCompileUnit(parser.compileUnit()) as ProgramNode;
        }

        private void LoadProg()
        {
            string content;

            using (StreamReader sr = new StreamReader(PROGRAM_PATH))
            {
                content = sr.ReadToEnd();
            }

            llParser parser = new llParser(new CommonTokenStream(new llLexer(new AntlrInputStream(content))));
            parser.RemoveErrorListeners();
            parser.AddErrorListener(new ErrorListener(PROGRAM_PATH));

            this.Prog = new FunctionDefinitionVisitor(PROGRAM_PATH).VisitCompileUnit(parser.compileUnit()) as ProgramNode;
            parser.Reset();
            this.BuildAstVisitor = new BuildAstVisitor(this.Prog);
            this.Prog = this.BuildAstVisitor.VisitCompileUnit(parser.compileUnit()) as ProgramNode;
        }

        [TestCase("aCallsB(3)", 5)]
        [TestCase("id(5)", 5)]
        [TestCase("square(5)", 25)]
        [TestCase("fourtyTwo()", 42)]
        [TestCase("plusSeventeen(square(5))", 42)]
        [TestCase("fac(5)", 120)]
        [TestCase("facIter(5)", 120)]
        [TestCase("equalsArray()", 42)]
        [TestCase("equalsArrayNull()", 42)]
        [TestCase("equalsNullArrayNull()", 42)]
        [TestCase("notEqualsArrayNull()", 42)]
        [TestCase("notEqualsArray()", 42)]
        [TestCase("notEqualsNullArrayNull()", 42)]
        public void TestFunctions_1(string funCall, int expected)
        {
            if(!this.once)
            {
                this.once = true;
                this.LoadProg();
            }

            this.Execute(funCall);
            var value = this.Prog.Eval();

            Assert.AreEqual(expected, (value as IntLit).Value);
        }

        [Test]
        public void TestFunctions_2()
        {
            string filePath = "../../../programs/TestFile3.ll";
            string content;
            using(StreamReader sr = new StreamReader(filePath))
            {
                content = sr.ReadToEnd();
            }

            llParser parser = new llParser(new CommonTokenStream(new llLexer(new AntlrInputStream(content))));
            ProgramNode prog = new FunctionDefinitionVisitor(filePath).VisitCompileUnit(parser.compileUnit()) as ProgramNode;
            parser.Reset();
            BuildAstVisitor visitor = new BuildAstVisitor(prog);

            Assert.Throws<MissingReturnStatementException>(() => visitor.Visit(parser.program()));
        }
    }
}