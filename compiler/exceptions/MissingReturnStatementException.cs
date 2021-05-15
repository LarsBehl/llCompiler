using System;

namespace LL.Exceptions
{
    public class MissingReturnStatementException : BaseCompilerException
    {
        private static readonly string MESSAGE = "Missing return-statement in function";
        public string FunctionName { get; set; }
        public string ExpectedReturnType { get; set; }

        public MissingReturnStatementException(
            string functionName,
            string expectedReturnType,
            string currentFile,
            int line,
            int column
        ) : base(
            $"{MESSAGE}: {functionName};{Environment.NewLine}Expected type: {expectedReturnType}",
            currentFile,
            line,
            column
        )
        {
            this.ExpectedReturnType = expectedReturnType;
            this.FunctionName = functionName;
        }
    }
}