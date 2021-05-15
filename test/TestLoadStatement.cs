using NUnit.Framework;

using Antlr4.Runtime;

using LL.AST;
using LL.Exceptions;

namespace LL.Test
{
    [TestFixture]
    public class TestLoadStatement
    {
        private static readonly string UNKNOWN_FILE = "./programs/TestFile2.ll";
        private static readonly string SINGLE_LOAD = "./programs/TestFile4.ll";
        private static readonly string FUNCTION_CONFLICT = "./programs/TestFile5.ll";
        private static readonly string STRUCT_CONFLICT = "./programs/TestFile6.ll";
        private static readonly string DEEP_DEPENDENCY = "./programs/TestFile7.ll";

        private llParser Setup(string filePath)
        {
            return new llParser(new CommonTokenStream(new llLexer(new AntlrFileStream(filePath))));
        }

        [Test]
        public void TestLoadStatement1()
        {
            llParser parser = this.Setup(UNKNOWN_FILE);
            StructDefinitionVisitor visitor = new StructDefinitionVisitor(UNKNOWN_FILE);
            Assert.Throws<FileNotFoundException>(() => visitor.VisitCompileUnit(parser.compileUnit()));
        }

        [Test]
        public void TestLoadStatement2()
        {
            llParser parser = this.Setup(SINGLE_LOAD);
            StructDefinitionVisitor visitor = new StructDefinitionVisitor(SINGLE_LOAD);
            ProgramNode prog = visitor.VisitCompileUnit(parser.compileUnit()) as ProgramNode;

            // A single dependency should be loaded
            Assert.AreEqual(1, prog.Dependencies.Count);

            // No errors should occure when collecting the function definitions
            parser.Reset();
            prog = new FunctionDefinitionVisitor(SINGLE_LOAD, prog).VisitCompileUnit(parser.compileUnit()) as ProgramNode;
            // Only a single function is part of the root program
            Assert.AreEqual(1, prog.FunDefs.Count);

            parser.Reset();
            prog = new BuildAstVisitor(prog).VisitCompileUnit(parser.compileUnit()) as ProgramNode;
            // All functions should have a funtion body
            foreach(FunctionDefinition fundef in prog.FunDefs.Values)
            {
                Assert.NotNull(fundef.Body);
            }
        }

        [Test]
        public void TestLoadStatement3()
        {
            llParser parser = this.Setup(FUNCTION_CONFLICT);
            ProgramNode prog = new StructDefinitionVisitor(FUNCTION_CONFLICT).VisitCompileUnit(parser.compileUnit()) as ProgramNode;

            Assert.AreEqual(1, prog.Dependencies.Count);

            parser.Reset();
            FunctionDefinitionVisitor visitor = new FunctionDefinitionVisitor(FUNCTION_CONFLICT, prog);

            Assert.Throws<FunctionAlreadyDefinedException>(() => visitor.VisitCompileUnit(parser.compileUnit()));
        }

        [Test]
        public void TestLoadStatement4()
        {
            llParser parser = this.Setup(STRUCT_CONFLICT);
            StructDefinitionVisitor visitor = new StructDefinitionVisitor(STRUCT_CONFLICT);

            Assert.Throws<StructAlreadyDefinedException>(() => visitor.VisitCompileUnit(parser.compileUnit()));
        }

        [Test]
        public void TestLoadStatement5()
        {
            llParser parser = this.Setup(DEEP_DEPENDENCY);
            ProgramNode prog = new StructDefinitionVisitor(DEEP_DEPENDENCY).VisitCompileUnit(parser.compileUnit()) as ProgramNode;

            parser.Reset();
            prog = new FunctionDefinitionVisitor(DEEP_DEPENDENCY, prog).VisitCompileUnit(parser.compileUnit()) as ProgramNode;

            parser.Reset();
            prog = new BuildAstVisitor(prog).VisitCompileUnit(parser.compileUnit()) as ProgramNode;

            foreach(FunctionDefinition fundef in prog.FunDefs.Values)
                Assert.NotNull(fundef.Body);
            
            foreach(LoadStatement dep in prog.Dependencies.Values)
                Assert.NotNull(dep.Program);
        }
    }
}