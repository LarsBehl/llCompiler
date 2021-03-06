namespace LL.Exceptions
{
    public class FileNotFoundException : BaseCompilerException
    {
        private static readonly string FILE_NOT_FOUND = "File not found";
        public string FileToFind { get; set; }

        public FileNotFoundException(string fileToFind, string currentFile, int line, int column)
        : base($"{FILE_NOT_FOUND}: {fileToFind}", currentFile, line, column)
            => this.FileToFind = fileToFind;
    }
}