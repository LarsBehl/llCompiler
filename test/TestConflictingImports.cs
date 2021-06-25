using System.Collections.Generic;

using NUnit.Framework;

using Antlr4.Runtime;

using LL.AST;
using LL.Exceptions;
using LL.Helper;

namespace LL.Test
{
    [TestFixture]
    public class TestConflictingImports
    {
        private static readonly string FUNCTION_CONFLICT = "./programs/TestFile16.ll";
        private llParser Setup(string filePath)
        {
            return new llParser(new CommonTokenStream(new llLexer(new AntlrFileStream(filePath))));
        }

        [Test]
        public void TestConflictingImports1()
        {
            StructDefinitionVisitor.ProgData = new Helper.ProgramData();
            llParser parser = this.Setup(FUNCTION_CONFLICT);
            StructDefinitionVisitor visitor = new StructDefinitionVisitor(FUNCTION_CONFLICT);
            ProgramNode node = visitor.VisitCompileUnit(parser.compileUnit()) as ProgramNode;
            parser.Reset();
            node.Parser = parser;
            StructDefinitionVisitor.ProgData.RootProgram = node;
            List<ProgramNode> nodes = StructDefinitionVisitor.ProgData.ContainsCircularDependency();

            foreach(ProgramNode prog in nodes)
                new FunctionDefinitionVisitor(prog).VisitCompileUnit(prog.Parser.compileUnit());

            Assert.Throws<ConflictingImportException>(() => CompilationHelper.ConflictingImports(node));
        }
    }
}