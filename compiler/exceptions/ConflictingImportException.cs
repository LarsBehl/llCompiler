namespace LL.Exceptions
{
    public class ConflictingImportException : BaseCompilerException
    {
        private static readonly string MESSAGE = "Conflict in loads";
        public string Name { get; set; }
        public string File1 { get; set; }
        public string File2 { get; set; }
        public string Type { get; set; }

        public ConflictingImportException(
            string name,
            string file1,
            string file2,
            string type,
            string fileName,
            int line,
            int column
        ) : base($"{MESSAGE} {type}: definition of {name} in {file1} and {file2}", fileName, line, column)
        {
            this.Name = name;
            this.Type = type;
            this.File1 = file1;
            this.File2 = file2;
        }
    }
}