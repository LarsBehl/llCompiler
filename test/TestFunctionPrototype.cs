using NUnit.Framework;

using System.Collections.Generic;

using Antlr4.Runtime;

using LL.AST;
using LL.Exceptions;

namespace LL.Test
{
    [TestFixture]
    public class TestFunctionPrototype
    {
        private static readonly string FILE_PATH = "./programs/TestFile10.ll";
        private static readonly string FUNCTION_CONFLICT = "./programs/TestFile11.ll";
        private static readonly string FUNCTION_IN_HEADER = "./programs/TestHeader2.llh";
        private static readonly string PROTO_IN_SOURCE = "./programs/TestFile12.ll";

        private llParser Setup(string filePath)
        {
            llParser result = new llParser(new CommonTokenStream(new llLexer(new AntlrFileStream(filePath))));
            result.RemoveErrorListeners();
            result.AddErrorListener(new ErrorListener(filePath));

            return result;
        }

        [Test]
        public void TestFunctionPrototype1()
        {
            StructDefinitionVisitor.ProgData = new Helper.ProgramData();
            llParser parser = this.Setup(FILE_PATH);
            StructDefinitionVisitor structVisitor = new StructDefinitionVisitor(FILE_PATH);
            ProgramNode rootProg = structVisitor.Visit(parser.compileUnit()) as ProgramNode;
            parser.Reset();

            FunctionDefinitionVisitor visitor = new FunctionDefinitionVisitor(rootProg);
            IAST ast = visitor.Visit(parser.compileUnit());

            Assert.NotNull(ast);
        }

        [Test]
        public void TestFunctionPrototype2()
        {
            StructDefinitionVisitor.ProgData = new Helper.ProgramData();
            llParser parser = this.Setup(FUNCTION_CONFLICT);
            StructDefinitionVisitor structVisitor = new StructDefinitionVisitor(FILE_PATH);
            ProgramNode rootProg = structVisitor.Visit(parser.compileUnit()) as ProgramNode;
            parser.Reset();
            rootProg.Parser = parser;
            StructDefinitionVisitor.ProgData.RootProgram = rootProg;

            List<ProgramNode> nodes = StructDefinitionVisitor.ProgData.ContainsCircularDependency();

            new FunctionDefinitionVisitor(nodes[0]).VisitCompileUnit(nodes[0].Parser.compileUnit());
            FunctionDefinitionVisitor visitor = new FunctionDefinitionVisitor(rootProg);
            
            Assert.Throws<FunctionAlreadyDefinedException>(() => visitor.Visit(parser.compileUnit()));
        }

        [Test]
        public void TestFunctionPrototype3()
        {
            StructDefinitionVisitor.ProgData = new Helper.ProgramData();
            llParser parser = this.Setup(FUNCTION_IN_HEADER);
            FunctionDefinitionVisitor visitor = new FunctionDefinitionVisitor(FUNCTION_IN_HEADER);

            Assert.Throws<IllegalOperationException>(() => visitor.Visit(parser.compileUnit()));
        }

        [Test]
        public void TestFunctionPrototype4()
        {
            StructDefinitionVisitor.ProgData = new Helper.ProgramData();
            llParser parser = this.Setup(PROTO_IN_SOURCE);
            FunctionDefinitionVisitor visitor = new FunctionDefinitionVisitor(PROTO_IN_SOURCE);

            Assert.Throws<IllegalOperationException>(() => visitor.Visit(parser.compileUnit()));
        }
    }
}