namespace LL.Exceptions
{
    public class NoValueException : BaseCompilerException
    {
        private static readonly string MESSAGE = "No value present";

        public NoValueException(string message, string fileName, int line, int column) : base($"{MESSAGE}: {message}", fileName, line, column)
        {

        }
    }
}