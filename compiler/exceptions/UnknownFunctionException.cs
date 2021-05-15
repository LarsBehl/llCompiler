namespace LL.Exceptions
{
    public class UnknownFunctionException : BaseCompilerException
    {
        private static readonly string MESSAGE = "Unknown function";
        public string FunctionName { get; set; }

        public UnknownFunctionException(string functionName, string currentFile, int line, int column)
        : base($"{MESSAGE}: {functionName}", currentFile, line, column)
            => this.FunctionName = functionName;
    }
}