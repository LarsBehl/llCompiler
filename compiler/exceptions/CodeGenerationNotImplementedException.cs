namespace LL.Exceptions
{
    public class CodeGenerationNotImplementedException : BaseCompilerException
    {
        private static readonly string MESSAGE = "Code generation not implemented for node";
        public string NodeName { get; set; }

        public CodeGenerationNotImplementedException(string nodeName, string currentFile, int line, int column)
        : base($"{MESSAGE}: {nodeName}", currentFile, line, column)
            => this.NodeName = nodeName;
    }
}