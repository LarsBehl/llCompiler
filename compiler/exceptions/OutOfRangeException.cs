namespace LL.Exceptions
{
    public class OutOfRangeException : BaseCompilerException
    {
        private static readonly string MESSAGE = "Index out of range";
        public int Range { get; set; }
        public int Index { get; set; }

        public OutOfRangeException(int range, int index, string currentFile, int line, int column)
        : base($"{MESSAGE}: range {range}, index {index}", currentFile, line, column)
        {
            this.Range = range;
            this.Index = index;
        }
    }
}