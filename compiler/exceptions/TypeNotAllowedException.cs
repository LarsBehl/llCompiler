namespace LL.Exceptions
{
    public class TypeNotAllowedException : BaseCompilerException
    {
        private static readonly string MESSAGE = "Type not allowed";
        public string TypeName { get; set; }

        public TypeNotAllowedException(string typeName, string currentFile, int line, int column)
        : base($"{MESSAGE}: {typeName}", currentFile, line, column)
            => this.TypeName = typeName;
    }
}