using NUnit.Framework;

using Antlr4.Runtime;

using LL.AST;
using LL.Types;

namespace LL.Test
{
    [TestFixture]
    public class TestGlobalVariableStatement
    {
        private readonly string currentFile = "UnitTests";
        private readonly IntType intType = new IntType();
        private readonly DoubleType doubleType = new DoubleType();
        private readonly CharType charType = new CharType();
        private readonly BooleanType boolType = new BooleanType();
        private readonly StructType structType = new StructType("Test");
        private readonly IntArrayType intArrayType = new IntArrayType();
        private readonly DoubleArrayType doubleArrayType = new DoubleArrayType();
        private readonly BoolArrayType boolArrayType = new BoolArrayType();
        private readonly CharArrayType charArrayType = new CharArrayType();

        private llParser Setup(string input)
        {
            return new llParser(new CommonTokenStream(new llLexer(new AntlrInputStream(input))));
        }

        [Test]
        public void TestGlobalVariableStatement1()
        {
            string input = "global x: int = 42; unused(): void { }";
            llParser parser = this.Setup(input);
            ProgramNode prog = new FunctionDefinitionVisitor(this.currentFile).Visit(parser.compileUnit()) as ProgramNode;

            Assert.AreEqual(1, prog.GlobalVariables.Count);

            parser.Reset();
            prog = new BuildAstVisitor(prog).Visit(parser.compileUnit()) as ProgramNode;

            Assert.AreEqual(1, prog.GlobalVariables.Count);

            foreach(GlobalVariableStatement globVar in prog.GlobalVariables.Values)
            {
                Assert.AreEqual(this.intType, globVar.Variable.Type);
                Assert.NotNull(globVar.Value);
            }
        }

        [Test]
        public void TestGlobalVariableStatement2()
        {
            string input = "global x: double = 42; unused(): void { }";
            llParser parser = this.Setup(input);
            ProgramNode prog = new FunctionDefinitionVisitor(this.currentFile).Visit(parser.compileUnit()) as ProgramNode;

            Assert.AreEqual(1, prog.GlobalVariables.Count);

            parser.Reset();
            prog = new BuildAstVisitor(prog).Visit(parser.compileUnit()) as ProgramNode;

            Assert.AreEqual(1, prog.GlobalVariables.Count);

            foreach(GlobalVariableStatement globVar in prog.GlobalVariables.Values)
            {
                Assert.AreEqual(this.doubleType, globVar.Variable.Type);
                Assert.NotNull(globVar.Value);
            }
        }

        [Test]
        public void TestGlobalVariableStatement3()
        {
            string input = "global x: bool = true; unused(): void { }";
            llParser parser = this.Setup(input);
            ProgramNode prog = new FunctionDefinitionVisitor(this.currentFile).Visit(parser.compileUnit()) as ProgramNode;

            Assert.AreEqual(1, prog.GlobalVariables.Count);

            parser.Reset();
            prog = new BuildAstVisitor(prog).Visit(parser.compileUnit()) as ProgramNode;

            Assert.AreEqual(1, prog.GlobalVariables.Count);

            foreach(GlobalVariableStatement globVar in prog.GlobalVariables.Values)
            {
                Assert.AreEqual(this.boolType, globVar.Variable.Type);
                Assert.NotNull(globVar.Value);
            }
        }

        [Test]
        public void TestGlobalVariableStatement4()
        {
            string input = "global x: char = 'c'; unused(): void { }";
            llParser parser = this.Setup(input);
            ProgramNode prog = new FunctionDefinitionVisitor(this.currentFile).Visit(parser.compileUnit()) as ProgramNode;

            Assert.AreEqual(1, prog.GlobalVariables.Count);

            parser.Reset();
            prog = new BuildAstVisitor(prog).Visit(parser.compileUnit()) as ProgramNode;

            Assert.AreEqual(1, prog.GlobalVariables.Count);

            foreach(GlobalVariableStatement globVar in prog.GlobalVariables.Values)
            {
                Assert.AreEqual(this.charType, globVar.Variable.Type);
                Assert.NotNull(globVar.Value);
            }
        }

        [Test]
        public void TestGlobalVariableStatement5()
        {
            string input = "global x: Test = new Test(); struct Test { x: int; }";
            llParser parser = this.Setup(input);
            ProgramNode prog = new StructDefinitionVisitor(this.currentFile).Visit(parser.compileUnit()) as ProgramNode;

            Assert.IsNotEmpty(prog.StructDefs);

            parser.Reset();
            prog = new FunctionDefinitionVisitor(prog).Visit(parser.compileUnit()) as ProgramNode;

            Assert.IsNotEmpty(prog.GlobalVariables);

            parser.Reset();
            prog = new BuildAstVisitor(prog).Visit(parser.compileUnit()) as ProgramNode;

            foreach(GlobalVariableStatement globVar in prog.GlobalVariables.Values)
            {
                Assert.AreEqual(this.structType, globVar.Variable.Type);
                Assert.NotNull(globVar.Value);
            }
        }

        [Test]
        public void TestGlobalVariableStatement6()
        {
            string input = "global x: int[] = new int[5]; unused(): void { }";
            llParser parser = this.Setup(input);
            ProgramNode prog = new FunctionDefinitionVisitor(this.currentFile).Visit(parser.compileUnit()) as ProgramNode;

            Assert.AreEqual(1, prog.GlobalVariables.Count);

            parser.Reset();
            prog = new BuildAstVisitor(prog).Visit(parser.compileUnit()) as ProgramNode;

            Assert.AreEqual(1, prog.GlobalVariables.Count);

            foreach(GlobalVariableStatement globVar in prog.GlobalVariables.Values)
            {
                Assert.AreEqual(this.intArrayType, globVar.Variable.Type);
                Assert.NotNull(globVar.Value);
            }
        }

        [Test]
        public void TestGlobalVariableStatement7()
        {
            string input = "global x: double[] = new double[5]; unused(): void { }";
            llParser parser = this.Setup(input);
            ProgramNode prog = new FunctionDefinitionVisitor(this.currentFile).Visit(parser.compileUnit()) as ProgramNode;

            Assert.AreEqual(1, prog.GlobalVariables.Count);

            parser.Reset();
            prog = new BuildAstVisitor(prog).Visit(parser.compileUnit()) as ProgramNode;

            Assert.AreEqual(1, prog.GlobalVariables.Count);

            foreach(GlobalVariableStatement globVar in prog.GlobalVariables.Values)
            {
                Assert.AreEqual(this.doubleArrayType, globVar.Variable.Type);
                Assert.NotNull(globVar.Value);
            }
        }

        [Test]
        public void TestGlobalVariableStatement8()
        {
            string input = "global x: bool[] = new bool[5]; unused(): void { }";
            llParser parser = this.Setup(input);
            ProgramNode prog = new FunctionDefinitionVisitor(this.currentFile).Visit(parser.compileUnit()) as ProgramNode;

            Assert.AreEqual(1, prog.GlobalVariables.Count);

            parser.Reset();
            prog = new BuildAstVisitor(prog).Visit(parser.compileUnit()) as ProgramNode;

            Assert.AreEqual(1, prog.GlobalVariables.Count);

            foreach(GlobalVariableStatement globVar in prog.GlobalVariables.Values)
            {
                Assert.AreEqual(this.boolArrayType, globVar.Variable.Type);
                Assert.NotNull(globVar.Value);
            }
        }

        [Test]
        public void TestGlobalVariableStatement9()
        {
            string input = "global x: char[] = new char[5]; unused(): void { }";
            llParser parser = this.Setup(input);
            ProgramNode prog = new FunctionDefinitionVisitor(this.currentFile).Visit(parser.compileUnit()) as ProgramNode;

            Assert.AreEqual(1, prog.GlobalVariables.Count);

            parser.Reset();
            prog = new BuildAstVisitor(prog).Visit(parser.compileUnit()) as ProgramNode;

            Assert.AreEqual(1, prog.GlobalVariables.Count);

            foreach(GlobalVariableStatement globVar in prog.GlobalVariables.Values)
            {
                Assert.AreEqual(this.charArrayType, globVar.Variable.Type);
                Assert.NotNull(globVar.Value);
            }
        }
    }
}