namespace ll.AST
{
    public class StructProperty : IAST
    {
        public string name { get; set; }

        public StructProperty(string name, ll.type.Type type) : base(type)
        {
            this.name = name;
        }
    }
}