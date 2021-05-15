namespace LL.Exceptions
{
    public class TypeMissmatchException : BaseCompilerException
    {
        private static readonly string MESSAGE = "Types are not implicitly convertable";
        public string ExpectedType { get; set; }
        public string ReceivedType { get; set; }

        public TypeMissmatchException(
            string expectedType,
            string receivedType,
            string currentFile,
            int line,
            int column
        ) : base(
            $"{MESSAGE}: expected {expectedType}, received {receivedType}",
            currentFile,
            line,
            column
        )
        {
            this.ExpectedType = expectedType;
            this.ReceivedType = receivedType;
        }
    }
}