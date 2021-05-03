namespace LL.Exceptions
{
    public class ArgumentCountException : BaseCompilerException
    {
        private static readonly string MESSAGE = "Expected more agruments of type";
        public string Type { get; set; }

        public ArgumentCountException(string type, string currentFile, int line, int column)
        : base($"{MESSAGE}: {type}", currentFile, line, column)
            => this.Type = type;
    }
}