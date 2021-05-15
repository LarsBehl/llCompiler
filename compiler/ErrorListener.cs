using System.IO;
using Antlr4.Runtime;
using LL.Exceptions;

namespace LL
{
    public class ErrorListener : BaseErrorListener
    {
        public string CurrentFile { get; set; }

        public ErrorListener(string currentFile) : base() => this.CurrentFile = currentFile;

        public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            throw new SyntaxErrorException(msg, offendingSymbol.Text, this.CurrentFile, line, charPositionInLine);
        }
    }
}