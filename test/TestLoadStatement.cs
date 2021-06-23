using System;

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
        private static readonly string FUNCTION_NOT_REACHABLE = "./programs/TestFile8.ll";
        private static readonly string STRUCT_DEEP = "./programs/TestFile9.ll";

        private llParser Setup(string filePath)
        {
            return new llParser(new CommonTokenStream(new llLexer(new AntlrFileStream(filePath))));
        }

        [Test]
        public void TestLoadStatement1()
        {
            StructDefinitionVisitor.ProgData = new Helper.ProgramData();
            llParser parser = this.Setup(UNKNOWN_FILE);
            StructDefinitionVisitor visitor = new StructDefinitionVisitor(UNKNOWN_FILE);
            Assert.Throws<FileNotFoundException>(() => visitor.VisitCompileUnit(parser.compileUnit()));
        }

        [Test]
        public void TestLoadStatement2()
        {
            StructDefinitionVisitor.ProgData = new Helper.ProgramData();
            llParser parser = this.Setup(SINGLE_LOAD);
            StructDefinitionVisitor visitor = new StructDefinitionVisitor(SINGLE_LOAD);
            ProgramNode prog = visitor.VisitCompileUnit(parser.compileUnit()) as ProgramNode;
            parser.Reset();
            prog.Parser = parser;
            StructDefinitionVisitor.ProgData.RootProgram = prog;
            StructDefinitionVisitor.ProgData.ContainsCircularDependency(out var nodes);

            // A single dependency should be loaded
            Assert.AreEqual(1, prog.Dependencies.Count);

            foreach (ProgramNode node in nodes)
            {
                // No errors should occure when collecting the function definitions
                new FunctionDefinitionVisitor(SINGLE_LOAD, node).VisitCompileUnit(node.Parser.compileUnit());

                node.Parser.Reset();
                new BuildAstVisitor(node).VisitCompileUnit(node.Parser.compileUnit());
            }
            // All functions should have a funtion body
            foreach (FunctionDefinition fundef in prog.FunDefs.Values)
            {
                Assert.NotNull(fundef.Body);
            }
        }

        [Test]
        public void TestLoadStatement3()
        {
            StructDefinitionVisitor.ProgData = new Helper.ProgramData();
            llParser parser = this.Setup(FUNCTION_CONFLICT);
            ProgramNode prog = new StructDefinitionVisitor(FUNCTION_CONFLICT).VisitCompileUnit(parser.compileUnit()) as ProgramNode;
            parser.Reset();

            prog.Parser = parser;
            Assert.AreEqual(1, prog.Dependencies.Count);

            StructDefinitionVisitor.ProgData.RootProgram = prog;
            StructDefinitionVisitor.ProgData.ContainsCircularDependency(out var nodes);

            for (int i = 0; i < nodes.Count - 1; i++)
            {
                new FunctionDefinitionVisitor(nodes[i]).VisitCompileUnit(nodes[i].Parser.compileUnit());
            }

            FunctionDefinitionVisitor visitor = new FunctionDefinitionVisitor(FUNCTION_CONFLICT, prog);

            Assert.Throws<FunctionAlreadyDefinedException>(() => visitor.VisitCompileUnit(parser.compileUnit()));
        }

        [Test]
        public void TestLoadStatement4()
        {
            StructDefinitionVisitor.ProgData = new Helper.ProgramData();
            llParser parser = this.Setup(STRUCT_CONFLICT);
            StructDefinitionVisitor visitor = new StructDefinitionVisitor(STRUCT_CONFLICT);

            Assert.Throws<StructAlreadyDefinedException>(() => visitor.VisitCompileUnit(parser.compileUnit()));
        }

        [Test]
        public void TestLoadStatement5()
        {
            StructDefinitionVisitor.ProgData = new Helper.ProgramData();
            llParser parser = this.Setup(DEEP_DEPENDENCY);
            ProgramNode prog = new StructDefinitionVisitor(DEEP_DEPENDENCY).VisitCompileUnit(parser.compileUnit()) as ProgramNode;
            parser.Reset();
            prog.Parser = parser;
            StructDefinitionVisitor.ProgData.RootProgram = prog;
            StructDefinitionVisitor.ProgData.ContainsCircularDependency(out var nodes);

            foreach (ProgramNode node in nodes)
            {
                new FunctionDefinitionVisitor(node).VisitCompileUnit(node.Parser.compileUnit());
                node.Parser.Reset();

                new BuildAstVisitor(node).VisitCompileUnit(node.Parser.compileUnit());
            }

            foreach (FunctionDefinition fundef in prog.FunDefs.Values)
                Assert.NotNull(fundef.Body);

            foreach (LoadStatement dep in prog.Dependencies.Values)
                Assert.NotNull(dep.Program);
        }

        [Test]
        public void TestLoadStatement6()
        {
            StructDefinitionVisitor.ProgData = new Helper.ProgramData();
            llParser parser = this.Setup(FUNCTION_NOT_REACHABLE);
            ProgramNode prog = new StructDefinitionVisitor(FUNCTION_NOT_REACHABLE).VisitCompileUnit(parser.compileUnit()) as ProgramNode;

            parser.Reset();
            prog = new FunctionDefinitionVisitor(FUNCTION_NOT_REACHABLE, prog).VisitCompileUnit(parser.compileUnit()) as ProgramNode;

            parser.Reset();
            BuildAstVisitor visitor = new BuildAstVisitor(prog);

            Assert.Throws<UnknownFunctionException>(() => visitor.VisitCompileUnit(parser.compileUnit()));
        }

        [Test]
        public void TestLoadStatement7()
        {
            StructDefinitionVisitor.ProgData = new Helper.ProgramData();
            llParser parser = this.Setup(STRUCT_DEEP);
            ProgramNode prog = new StructDefinitionVisitor(STRUCT_DEEP).VisitCompileUnit(parser.compileUnit()) as ProgramNode;

            parser.Reset();
            prog = new FunctionDefinitionVisitor(STRUCT_DEEP, prog).VisitCompileUnit(parser.compileUnit()) as ProgramNode;

            parser.Reset();
            prog = new BuildAstVisitor(prog).VisitCompileUnit(parser.compileUnit()) as ProgramNode;

            Assert.NotNull(prog);
            Assert.NotNull(prog.FunDefs);
            Assert.IsEmpty(prog.StructDefs);
        }
    }
}