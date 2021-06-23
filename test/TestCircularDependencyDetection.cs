using NUnit.Framework;

using Antlr4.Runtime;

using LL.AST;
using LL.Exceptions;
using LL.Helper;

namespace LL.Test
{
    [TestFixture]
    public class TestCircularDependencyDetection
    {
        private static readonly string ROOT = "./programs/TestFile13.ll";

        private llParser Setup(string filePath)
        {
            return new llParser(new CommonTokenStream(new llLexer(new AntlrFileStream(filePath))));
        }

        [Test]
        public void TestCircularDependencyDetection1()
        {
            StructDefinitionVisitor.ProgData = new ProgramData();
            llParser parser = this.Setup(ROOT);
            StructDefinitionVisitor visitor = new StructDefinitionVisitor(ROOT);
            ProgramNode prog = visitor.VisitCompileUnit(parser.compileUnit()) as ProgramNode;
            parser.Reset();
            prog.Parser = parser;
            StructDefinitionVisitor.ProgData.RootProgram = prog;
            bool hasCircular = StructDefinitionVisitor.ProgData.ContainsCircularDependency(out var nodes);

            Assert.True(hasCircular);
        }
    }
}