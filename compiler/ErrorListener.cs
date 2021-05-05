using System.IO;
using Antlr4.Runtime;
using LL.Exceptions;

namespace LL
{
    public class ErrorListener : BaseErrorListener
    {
        // TODO add currentFile
        public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            throw new SyntaxErrorException(msg, offendingSymbol.Text, null, line, charPositionInLine);
        }
    }
}