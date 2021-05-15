namespace LL.Exceptions
{
    public class CodeAlreadyGeneratedException : BaseCompilerException
    {
        private static readonly string MESSAGE = "Code already generated for";

        public CodeAlreadyGeneratedException(string fileLocation) : base($"{MESSAGE}: {fileLocation}", fileLocation, -1, -1)
        {

        }
    }
}