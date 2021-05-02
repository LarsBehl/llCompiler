namespace LL.Exceptions
{
    public class VariableAlreadyDefinedException : BaseCompilerException
    {
        private static readonly string MESSAGE = "Variable is already defined";
        public string VariableName { get; set; }

        public VariableAlreadyDefinedException(string variableName, string currentFile, int line, int column)
        : base($"{MESSAGE}: {variableName}", currentFile, line, column)
            => this.VariableName = variableName;
    }
}