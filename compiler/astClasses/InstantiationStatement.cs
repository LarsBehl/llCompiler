using ll.type;

namespace ll.AST
{
    public class InstantiationStatement : IAST
    {
        public string name { get; set; }

        public InstantiationStatement(string name, type.Type type) : base(type)
        {
            this.name = name;
        }
    }
}