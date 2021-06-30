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
        private llParser Setup(string filePath)
        {
            return new llParser(new CommonTokenStream(new llLexer(new AntlrFileStream(filePath))));
        }

        [TestCase("./programs/TestFile16.ll")]
        public void TestConflictingImports1(string fileName)
        {
            StructDefinitionVisitor.ProgData = new Helper.ProgramData();
            llParser parser = this.Setup(fileName);
            StructDefinitionVisitor visitor = new StructDefinitionVisitor(fileName);
            ProgramNode node = visitor.VisitCompileUnit(parser.compileUnit()) as ProgramNode;
            parser.Reset();
            node.Parser = parser;
            StructDefinitionVisitor.ProgData.RootProgram = node;
            List<ProgramNode> nodes = StructDefinitionVisitor.ProgData.ContainsCircularDependency();

            foreach(ProgramNode prog in nodes)
                new FunctionDefinitionVisitor(prog).VisitCompileUnit(prog.Parser.compileUnit());

            Assert.Throws<ConflictingImportException>(() => CompilationHelper.ConflictingImportedFunctions(node));
        }
    }
}