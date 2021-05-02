namespace LL.Exceptions
{
    public class PropertyAlreadyDefinedException : BaseCompilerException
    {
        private static readonly string MESSAGE = "Property already defined";
        public string PropertyName { get; set; }
        public string StructName { get; set; }

        public PropertyAlreadyDefinedException(
            string propertyName,
            string structName,
            string currentFile,
            int line,
            int column
        ): base(
            $"{MESSAGE}: {propertyName} in {structName}",
            currentFile,
            line,
            column
        )
        {
            this.PropertyName = propertyName;
            this.StructName = structName;
        }
    }
}