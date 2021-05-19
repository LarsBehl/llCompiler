using NUnit.Framework;

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

        private llParser Setup(string filePath)
        {
            return new llParser(new CommonTokenStream(new llLexer(new AntlrFileStream(filePath))));
        }

        [Test]
        public void TestFunctionPrototype1()
        {
            llParser parser = this.Setup(FILE_PATH);
            FunctionDefinitionVisitor visitor = new FunctionDefinitionVisitor(FILE_PATH);
            IAST ast = visitor.Visit(parser.compileUnit());

            Assert.NotNull(ast);
        }

        [Test]
        public void TestFunctionPrototype2()
        {
            llParser parser = this.Setup(FUNCTION_CONFLICT);
            FunctionDefinitionVisitor visitor = new FunctionDefinitionVisitor(FUNCTION_CONFLICT);
            
            Assert.Throws<FunctionAlreadyDefinedException>(() => visitor.Visit(parser.compileUnit()));
        }
    }
}