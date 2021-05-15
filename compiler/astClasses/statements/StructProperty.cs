namespace LL.AST
{
    public class StructProperty : IAST
    {
        public string Name { get; set; }

        public StructProperty(string name, LL.Types.Type type, int line, int column) : base(type, line, column)
        {
            this.Name = name;
        }
    }
}