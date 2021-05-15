namespace LL.Exceptions
{
    public class UnknownOperatorException : BaseCompilerException
    {
        private static readonly string MESSAGE = "Unknown operator";
        public string Operator { get; set; }

        public UnknownOperatorException(string @operator, string currentFile, int line, int column)
        : base($"{MESSAGE}: {@operator}", currentFile, line, column)
            => this.Operator = @operator;
    }
}