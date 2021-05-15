namespace LL.Exceptions
{
    public class StructAlreadyDefinedException : BaseCompilerException
    {
        private static readonly string MESSAGE = "Struct is already defined";
        public string StructName { get; set; }

        public StructAlreadyDefinedException(string structName, string currentFile, int line, int column)
        : base($"{MESSAGE}: {structName}", currentFile, line, column)
            => this.StructName = structName;
    }
}