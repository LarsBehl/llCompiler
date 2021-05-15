using System;

namespace LL.Exceptions
{
    public class SyntaxErrorException : BaseCompilerException
    {
        private static readonly string MESSAGE = "Encountered a syntax error";
        public string SyntaxErrorMessage { get; set; }
        public string Token { get; set; }

        public SyntaxErrorException(string message, string token, string currentFile, int line, int column)
        : base($"{MESSAGE}:{Environment.NewLine}\t{message}", currentFile, line, column)
        {
            this.SyntaxErrorMessage = message;
            this.Token = token;
        }
    }
}