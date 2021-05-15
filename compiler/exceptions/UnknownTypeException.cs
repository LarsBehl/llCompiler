namespace LL.Exceptions
{
    public class UnknownTypeException : BaseCompilerException
    {
        private static readonly string MESSAGE = "Unknown type";
        public string TypeName { get; set; }

        public UnknownTypeException(string typeName, string currentFile, int line, int column)
        : base($"{MESSAGE}: {typeName}", currentFile, line, column)
            => this.TypeName = typeName;
    }
}