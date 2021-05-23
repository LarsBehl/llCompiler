namespace LL.Exceptions
{
    public class IllegalOperationException : BaseCompilerException
    {
        private static readonly string MESSAGE = "Illegal operation";

        public IllegalOperationException(string message, string fileName, int line, int column) : base($"{MESSAGE}: {message}", fileName, line, column)
        {
            
        }
    }
}