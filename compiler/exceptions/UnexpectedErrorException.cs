namespace LL.Exceptions
{
    public class UnexpectedErrorException : BaseCompilerException
    {
        private static readonly string MESSAGE = "An unexpected error occure";

        public UnexpectedErrorException(string currentFile): this(currentFile, -1, -1)
        {
            
        }

        public UnexpectedErrorException(string currentFile, int line, int column) : base(MESSAGE, currentFile, line, column)
        {

        }
    }
}