using ll.type;

namespace ll.AST
{
    public class InstantiationStatement : IAST
    {
        public string name { get; set; }

        public InstantiationStatement(string name, type.Type type, int line, int column) : base(type, line, column)
        {
            this.name = name;
        }
    }
}