namespace LL.Exceptions
{
    public class UnknownVariableException : BaseCompilerException
    {
        private static readonly string MESSAGE = "Unknown variable";
        public string VariableName { get; set; }

        public UnknownVariableException(string variableName, string currentFile, int line, int column)
        : base($"{MESSAGE}: {variableName}", currentFile, line, column)
            => this.VariableName = variableName;
    }
}