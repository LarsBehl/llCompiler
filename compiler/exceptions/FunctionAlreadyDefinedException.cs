namespace LL.Exceptions
{
    public class FunctionAlreadyDefinedException : BaseCompilerException
    {
        private static readonly string MESSAGE = "Function already defined";
        public string FunctionName { get; set; }

        public FunctionAlreadyDefinedException(string functionName, string currentFile, int line, int column)
        : base($"{MESSAGE}: {functionName}", currentFile, line, column)
            => this.FunctionName = functionName;
    }
}