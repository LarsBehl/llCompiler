namespace LL.Exceptions
{
    public class UnknownCharacterException : BaseCompilerException
    {
        private static readonly string MESSAGE = "Unknown character";
        public string Character { get; set; }

        public UnknownCharacterException(string character, string fileName, int line, int column)
            : base($"{MESSAGE}: {character}", fileName, line, column) => this.Character = character;
    }
}