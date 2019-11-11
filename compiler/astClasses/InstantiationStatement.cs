namespace ll.AST
{
    public class InstantiationStatement : IAST
    {
        public string name { get; set; }

        public InstantiationStatement(string name, ll.type.Type type) : base(type)
        {
            this.name = name;
        }
    }
}