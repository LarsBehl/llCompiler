using System;

namespace ll.Exceptions
{
    public abstract class BaseCompilerException : Exception
    {
        public int Line { get; set; }
        public int Column { get; set; }
        public string FileName { get; set; }

        public BaseCompilerException(string message, string fileName, int line, int column)
            : base($"\t{message}; In file \"{fileName}\" line {line}:{column}")
        {
            this.Line = line;
            this.Column = Column;
            this.FileName = fileName;
        }
    }
}