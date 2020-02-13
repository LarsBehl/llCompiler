namespace ll.AST
{
    public class StructProperty : IAST
    {
        public string name { get; set; }

        public StructProperty(string name, ll.type.Type type, int line, int column) : base(type, line, column)
        {
            this.name = name;
        }
    }
}